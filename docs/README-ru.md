# Zoo World

> **Язык:** [English](../README.md) | **Русский**

Небольшой top-down 3D симулятор жизни. Сделан как тестовое задание, с упором на **архитектуру животных**: добавление нового животного должно быть простым, путём добавлением одного ScriptableObject-ассета или в крайнем случае нескольких небольших новых классов, учитывая приниц Open-Closed, но никак не правки существующего кода.

**Unity 2022.3.44f1** · URP · Zenject/Extenject · UniRx · UniTask · DoTween · TextMeshPro · uGUI + MVVM · `UnityEngine.Pool`

---

## Геймплей

![Геймплей](gameplay.gif)

Каждые 1–2 секунды появляется животное (лягушка - зелёная сфера, змея - красный куб) и перемещается по платформе с помощью физики. Животные меняют направление движения, если сталкиваются с препятствиями, разворачиваются если выходят за пределы камеры и взаимодействуют по пищевой цепи:

- **Хищник × Жертва** — жертву съедают, и она возвращается в пул; над хищником всплывает лейбл **«Tasty!»**.
- **Хищник × Хищник** — один из них погибает у выживщего хищника всплывает **«Tasty!»**.
- **Жертва × Жертва** — оба животных отталкиваются друг от друга.

Реактивный счётчик смертей (погибшие жертвы / хищники) — в правом верхнем углу. По умолчанию есть два вида: **Лягушка** (жертва, прыгает) и **Змея** (хищник, движется линейно).

---

## Стек

| Область | Выбор |
|---|---|
| DI | Zenject/Extenject — инсталлеры, фабрики, без синглтонов |
| Реактивность | UniRx — `ReactiveProperty`, `Observable.EveryFixedUpdate`, `CompositeDisposable` |
| Async | UniTask — луп спавна и жизненный цикл лейбла (с `CancellationToken`) |
| Анимация | DoTween — scale при спавне/смерти, всплытие и затухание «Tasty!» |
| Данные | ScriptableObject — конфиги животных и движения |
| UI | uGUI + MVVM + реактивные счётчики |
| Пул | `UnityEngine.Pool.ObjectPool<T>` — без `Instantiate`/`Destroy` в рантайме |

---

## Архитектура вкратце

- **Логика написана как POCO, а View это тонкие MonoBehaviour.** `Animal` это обычный C#-оркестратор: он владеет своими коллабораторами (движение, здоровье, FSM) и делегирует им работу. `AnimalView` почти ничего не делает сам, он только держит `Rigidbody` и `Collider` и пробрасывает столкновения в логику.
- **Один Update на всю игру.** `UpdateHandler` подписывается на `Observable.EveryFixedUpdate` и в одном месте тикает всех зарегистрированных `IFixedTickTarget`. Отдельных `Update()` на каждом животном нет, поэтому тысяча зверей не превращается в тысячу апдейтов.
- **Композиция вместо наследования.** Никакого дерева `Animal → Prey → Frog` тут нет. Вид животного собирается из трёх частей: данные, стратегия движения и роль.
- **Единый composition root.** Всё поднимается из одного Zenject `SceneContext`: пять небольших инсталлеров и линейный bootstrap на UniTask. Глобального game-FSM нет, конечный автомат используется только для состояний самого животного (`Moving`, `ReturningToScreen`, `Dead`).
- **Сборки выстроены в однонаправленный граф зависимостей** (`Common → Data → Gameplay / UI → Infrastructure`). Gameplay и UI напрямую друг о друге не знают, они общаются через контракты в `Common` (`IDeathReporter`, `ITastyLabelSpawner`, `IFixedTickRegistry`), а конкретные реализации связывает уже слой Infrastructure.

```
Assets/_Project/Scripts/
├── Common/          # enum-ы, контракты, ScreenBounds, RandomDirection
├── Data/            # ScriptableObject: AnimalDefinition, MovementConfig, GameSettings
├── Gameplay/
│   ├── Animals/     # Base (Animal, AnimalView, Health), States (FSM), Factory
│   ├── Movement/    # Strategies, Builders, MovementStrategyFactory
│   ├── FoodChain/   # AnimalFoodChainService + Rules
│   ├── Spawn/       # AnimalSpawner (UniTask), интервал и взвешенный выбор
│   └── Pool/        # AnimalPoolService, AnimalPoolLifecycleCoordinator
├── UI/              # Model / ViewModel / View (MVVM), TastyLabel
└── Infrastructure/  # GameBootstrap, UpdateHandler, Installers
```

---

## Паттерны проектирования

| Паттерн | Где применяется |
|---|---|
| **Dependency Injection** | Zenject `SceneContext` + пять инсталлеров; конструкторная инъекция везде, инъекция `IEnumerable<>` для правил/билдеров |
| **Композиция вместо наследования** | `Animal` оркестрирует коллабораторов (движение, здоровье, FSM, вью) вместо иерархии классов |
| **Strategy** | `IMovementStrategy` → `LinearMovementStrategy` / `JumpMovementStrategy` |
| **Builder** | `IMovementStrategyBuilder` собирает стратегию из своего `MovementConfig`, регистрируется в инсталлере |
| **Factory** | `MovementStrategyFactory` (диспетчеризация по словарю, без `switch`) и `AnimalFactory` |
| **State (FSM)** | `IAnimalState` + `AnimalStateMachine` (`Moving` / `ReturningToScreen` / `Dead`), используется только у животного |
| **Набор правил / Chain of Responsibility** | `AnimalFoodChainService` применяет первое подходящее `ICollisionRule` |
| **Object Pool** | `AnimalPoolService` и `TastyLabelPool` поверх `UnityEngine.Pool.ObjectPool<T>` |
| **MVVM** | `AnimalStatisticsService` (Model) → `DeathCounterViewModel` → `DeathCounterView` |
| **Observer / Reactive** | UniRx `ReactiveProperty`, `Observable.EveryFixedUpdate` и сигналы столкновений/смерти через `event` |
| **Template Method** | общие базы `DirectedMovementStrategyBase` и `PredatorFeedingRule` (переиспользование кода, не полиморфизм видов) |
| **Registry** | `AnimalCollisionRegistry` находит `Animal` по столкнувшемуся `Rigidbody` |
| **Mediator / Coordinator** | `AnimalPoolLifecycleCoordinator` связывает пул, реестр тика, пищевую цепь и смерть |
| **Update manager** | `UpdateHandler` тикает каждый `IFixedTickTarget` из одного `EveryFixedUpdate` |
| **Dependency Inversion** | межсборочные контракты в `Common` (`IDeathReporter`, `ITastyLabelSpawner`, `IFixedTickRegistry`) |
| **Data-driven конфигурация** | ScriptableObject (`AnimalDefinition`, `MovementConfig`, `GameSettings`, `AnimalRegistry`) |

---

## С чего начать чтение

1. **[`GameBootstrap`](../Assets/_Project/Scripts/Infrastructure/GameBootstrap.cs)**. Точка входа (`IInitializable`): прогревает пулы и запускает луп спавна.
2. **[`Installers]`(../Assets/_Project/Scripts/Infrastructure/Installers)**. Здесь видно, как всё связано между собой (`GameSettings → Gameplay → Animal → Spawn → UI`).
3. **[`AnimalFactory`](../Assets/_Project/Scripts/Gameplay/Animals/Factory/AnimalFactory.cs)**. Собирает живого `Animal` из `AnimalDefinition`.
4. **[`IMovementStrategy`](../Assets/_Project/Scripts/Gameplay/Movement/Strategies/IMovementStrategy.cs)** и **[`MovementStrategyFactory`](../Assets/_Project/Scripts/Gameplay/Movement/MovementStrategyFactory.cs)**. Здесь работают паттерны Strategy и Builder, а диспетчеризация идёт без `switch`.
5. **[`AnimalFoodChainService`](../Assets/_Project/Scripts/Gameplay/FoodChain/AnimalFoodChainService.cs)** и **[`ICollisionRule`](../Assets/_Project/Scripts/Gameplay/FoodChain/Rules/ICollisionRule.cs)**. Разрешение столкновений через набор data-driven правил, которые смотрят только на `AnimalRole`.
6. **[`AnimalStatisticsService`](../Assets/_Project/Scripts/UI/Model/AnimalStatisticsService.cs)**. Реактивный словарь, который стоит за счётчиком в MVVM.

---

## Как добавить новое животное

Весь дизайн подчинён одному вопросу: насколько легко добавить новое животное, не трогая существующий код. Разберём три случая, от самого простого к более сложному.

**Случай 1. Новый вид с уже существующим поведением** (например, ещё одна прыгающая жертва). Достаточно создать новый ассет `AnimalDefinition` (роль, HP, цвет, префаб, вес спавна), указать в нём один из уже существующих `MovementConfig` и добавить вид в `AnimalRegistry`. Кода при этом писать вообще не нужно.

**Случай 2. Новое движение** (например, летающая птица). Добавляем значение в enum `MovementType` и свой `MovementConfig` (только с теми полями, которые нужны этому движению), пишем `IMovementStrategy` и `IMovementStrategyBuilder`, после чего регистрируем билдер одной строкой в `AnimalInstaller`. Существующие стратегии и фабрика при этом не меняются, потому что фабрика берёт билдеры прямо из контейнера.

**Случай 3. Новое взаимодействие** (например, падальщик, который не трогает хищников). Добавляем новое `ICollisionRule` и биндим его в `AnimalInstaller`. Старые правила остаются как есть, а `AnimalFoodChainService` просто применяет первое подходящее.

---

## Ключевое

- **DI без синглтонов.** Никакого `static`-состояния и никакого service locator. Жизненным циклом объектов управляет контейнер через `IInitializable` и `IDisposable`.
- **Open-Closed повсюду.** Новое движение или взаимодействие сводится к одному небольшому классу и его регистрации, а существующий код при этом не трогается.
- **Ноль проверок на конкретный вид.** Ветвление идёт только по `AnimalRole`, в проекте нет ни одного `is Frog` или `switch` по типам.
- **Пул объектов** для животных и лейблов, благодаря чему при спавне не бывает всплесков GC.
- **Физика** отвечает за движение и столкновения (Rigidbody). DoTween нужен только для визуального полиша и никогда для перемещения животных.

---

## Требования и запуск

- **Unity 2022.3.44f1** (URP).
- UniTask и UniRx подключены по git-URL в `Packages/manifest.json` и подтягиваются автоматически при первом открытии проекта (нужен интернет).
- Откройте проект, загрузите сцену `Assets/_Project/Scenes/Game.unity` и нажмите **Play**.
- Если DoTween попросит первичную настройку, выполните `Tools → Demigiant → DOTween Utility Panel → Setup DOTween…`.
