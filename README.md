# TestInfra

Playwright (.NET) end-to-end test projects that compare different ways of structuring E2E tests (Baseline, Page Objects V1-V4, Screenplay). All suites test the same webshop, **ShopDemo**, and implement the same user journeys, so the trade-offs of each structure become visible. The webshop itself lives in the separate repository `master-thesis-demo-app`; this repository contains only the test infrastructure.

See [`docs/e2e-test-structure-poc.md`](docs/e2e-test-structure-poc.md) for the comparison and [`docs/slidev_presentation_E2E_Testing_Structure_with_Page_Object_Models.md`](docs/slidev_presentation_E2E_Testing_Structure_with_Page_Object_Models.md) for the presentation.

## Prerequisites

- .NET SDK 10
- A running ShopDemo instance. Start it from the `master-thesis-demo-app` repository (`dotnet run --project ShopDemo.Server`, or `docker compose up --build`).
- Playwright browsers: build the solution once, then run `pwsh TestInfra.Baseline/bin/Debug/net10.0/playwright.ps1 install chromium`.

## Run the tests

```powershell
dotnet restore TestInfra.slnx --source https://api.nuget.org/v3/index.json
dotnet build TestInfra.slnx --no-restore
dotnet test TestInfra.Baseline
```

Tests reach the app through `SHOPDEMO_BASE_URL` (default `https://localhost:53246/`, the app's launch-profile URL). For the docker compose stack use `http://localhost:5178`:

```powershell
$env:SHOPDEMO_BASE_URL = "http://localhost:5178"
dotnet test TestInfra.Pom.V4
```

Each suite can be run on its own: `TestInfra.Baseline`, `TestInfra.Pom.V1`, `TestInfra.Pom.V2`, `TestInfra.Pom.V3`, `TestInfra.Pom.V4`, `TestInfra.Screenplay`. `TestInfra.Generated` is reserved for LLM-generated Playwright tests (none committed yet).

## Projects

The solution has independent xUnit v3 projects using `Microsoft.Playwright.Xunit.v3` 1.63.0 and xUnit v3 3.2.2:

- **TestInfra.Shared** - common Playwright `PageTest` setup, base URL, demo data, and `TestDatabase.ResetAsync()`.
- **TestInfra.Baseline** - direct Playwright tests without POM or Screenplay abstractions.
- **TestInfra.Pom.V1** - hand-written page objects and POM scenarios.
- **TestInfra.Pom.V2** - independent attribute-bound page objects, reflective factory, locator binder, and tab scenarios. See [`TestInfra.Pom.V2/docs`](TestInfra.Pom.V2/docs).
- **TestInfra.Pom.V3** - independent compile-time page objects: static-abstract-interface-based generic creation, `Required<T>`/`Optional<T>` typed locators, no reflection. See [`TestInfra.Pom.V3/docs/architecture.md`](TestInfra.Pom.V3/docs/architecture.md).
- **TestInfra.Pom.V4** - V2 plus typed control wrappers (`Button`, `TextField`, `Select`, ...) that encapsulate MudBlazor interaction behaviour; see the [test-creation guide](TestInfra.Pom.V4/docs/test-creation-guide.md); work in progress.
- **TestInfra.Otel** — OpenTelemetry PoC: traces V4 user interactions and page-object binds as spans, exported to the console and/or an OTLP collector (e.g. the Jaeger container in `docker-compose.yml`; see `TestInfra.Pom.V4/otel.runsettings`).
- **TestInfra.Screenplay** - actors, abilities, tasks, questions, and matching scenarios.
- **TestInfra.Generated** - reserved for LLM-generated Playwright tests; currently contains no tests.

The Baseline suite keeps navigation, selectors, and interactions directly in each test to provide a comparison for code length and duplication. The POM V1 suite uses `PomPageTestBase`, which inherits `ShopDemoPageTest` to reuse its browser context options and provides a lazy `Pages` property; the page-object graph belongs to that test's `PageTest.Page` and is never shared across tests. V2, V3, and V4 deliberately do not reference V1 (V4 extends the V2 design). A lazy `GetPage<T>()` alternative is documented in [`docs/pom-page-object-factory.md`](docs/pom-page-object-factory.md); it is a proposal, not the current POM implementation.

### Reuse in the test patterns

- **POM V1:** For example `LoginPage.SignInAsync` and `CheckoutPage.PlaceOrderAsync` are reused in multiple shopper/manager flows; each page object keeps its UI actions and selectors together.
- **Screenplay:** For example `SignIn`, `AddProductToCart`, and `CompleteCheckout` are composed in multiple scenarios. The same `OrderIsVisible` question checks their outcomes.
- **Baseline:** The same login, cart, and checkout interactions are repeated inline, showing what the abstractions remove.

## Test isolation

State lives in the app's database, so the suites call `POST /api/test/reset` before every test (`ShopDemoPageTest` / `ShopDemoPageTestV4`) and run without parallelization; the app must run with `Testing__EnableResetEndpoint=true` (the default). Set `SHOPDEMO_RESET_DB=false` to keep data between tests. Each Playwright test uses a fresh browser context. Lists are filled from the API after navigation; assert with retrying expectations (or `LocatorExtensions.IsEventuallyVisibleAsync`) instead of reading a count immediately after a click.

## Demo accounts used by the tests

All demo accounts use the password `1234`: shopper `demo@test.com` and store manager `admin@test.com`. The full list of accounts, coupons, test ids, and the API is documented in the app repository (`docs/application.md`).
