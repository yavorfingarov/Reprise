# Changelog

## [3.7.0] - 2023-07-16
### Added
- Mappers
### Fixed
- FastEndpoints benchmarks

## [3.6.0] - 2023-07-12
### Added
- Jobs
### Changed
- Bumped FluentValidation to the latest version

## [3.5.0] - 2023-04-30
### Added
- Rate limiting

## [3.4.2] - 2023-02-20
### Changed
- Bumped FluentValidation to the latest version

## [3.4.1] - 2023-02-16
### Fixed
- Wording in readme
- Updated dependencies

## [3.4.0] - 2023-02-05
### Added
- SourceLink
- Deterministic build

## [3.3.1] - 2023-02-03
### Added
- More information in the readme

## [3.3.0] - 2023-02-01
### Added
- Option to publish events and wait for the handlers to finish execution.

## [3.2.0] - 2023-01-20
### Added
- Events

## [3.1.6] - 2022-12-20
### Fixed
- Carter benchmarks

## [3.1.5] - 2022-12-18
### Added
- API reference
- More benchmarks
### Changed
- Better logging performance

## [3.1.4] - 2022-12-12
### Fixed
- Readme wording on filters

## [3.1.3] - 2022-12-11
### Added
- Benchmarks

## [3.1.2] - 2022-12-04
### Fixed
- Missing null check in `UseExceptionHandling()`

## [3.1.1] - 2022-11-30
### Fixed
- Wrongfully try to set the response status code when an exception is thrown while 
handling an exception and the response has already started.

## [3.1.0] - 2022-11-27
### Added
- Filter ordering (.NET 7 only)
### Changed
- Endpoints can have multiple filters added via an attribute (.NET 7 only)
- Validation filter validates all validatable parameters not only the first one (.NET 7 only)

## [3.0.0] - 2022-11-26
### Added
- Extended OpenAPI support with Produces, Name and ExcludeFromDescription attributes
- NuGet package icon
### Changed
- Removed the public properties from all attributes (BREAKING)
- Validation filter can handle nullable reference types (.NET 7 only)

## [2.2.0] - 2022-11-23
### Added
- Filters (.NET 7 only)
- Validation filter (.NET 7 only)
### Fixed
- Bug in exception handler causing it to log only to a single logger provider when multiple are available

## [2.1.0] - 2022-11-21
### Added
- Validation
- Exception handling
- Target framework .NET 7

## [2.0.0] - 2022-11-19
### Added
- Option to require authorization for all endpoints
- Custom OpenAPI tags
### Changed
- `MapEndpoints(assembly)` is removed (BREAKING)
- Endpoint discovery now happens while configuring services

## [1.1.0] - 2022-10-30
### Added
- Binding a strongly-typed hierarchical configuration

## [1.0.3] - 2022-10-22
### Changed
- Enabled stricter code analysis

## [1.0.2] - 2022-10-21
### Added
- Argument null checks for all public methods
### Fixed
- Mistakes in readme
- Minor refactoring

## [1.0.1] - 2022-10-20
### Changed
- Extended the readme

## [1.0.0] - 2022-10-19
- Initial release
