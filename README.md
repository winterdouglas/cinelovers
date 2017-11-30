### Build instructions

- Restore nuget packages
- Symply build and run

### Running unit tests

- You'll need a test adapter to run the unit tests. Using Windows I'd suggest the [NUnit 3 Test Adapter](https://marketplace.visualstudio.com/items?itemName=NUnitDevelopers.NUnit3TestAdapter "NUnit 3 Test Adapter")

### List of third-party dependencies

- **ReactiveUI** (https://github.com/reactiveui/ReactiveUI) - This is a functional reactive model-view-viewmodel (MVVM) framework.
- **Akavache** (https://github.com/akavache/Akavache) - This is a key-value store for mobile and desktop apps. Used in this project to cache movies.
- **Refit** (https://github.com/paulcbetts/refit) - Type-safe REST library for C#. Its's being used to create the TMDb REST API client.
- **Splat** (https://github.com/reactiveui/splat) - This is a library with lots of features but in this project it's being used as a service location tool.
- **FFImageLoading** (https://github.com/luberda-molinet/FFImageLoading) - A library to load images that supports caching, SVG, animations, transformations and much more.
- **NUnit** (https://github.com/nunit/nunit) - One of the most popular testing frameworks, used here to write unit tests.
- **Moq** (https://github.com/moq/moq4) - This is a mocking framework. Used here in unit tests to be able to mock dependencies and keep unit tests clear and readable.
- **Newtonsoft.Json** (https://www.newtonsoft.com/json) - The most popular and high performant JSON framework for .NET. It's being used to serialize and deserialize movie request payloads and also to store cached data with akavache.
- **SnakeCase.JsonNet.Portable** (https://github.com/mrstebo/SnakeCase.JsonNet) - This is a library used to convert property names to snake_case during serialization.
