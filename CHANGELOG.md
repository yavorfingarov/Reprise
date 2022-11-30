# Changelog

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
- Extentended OpenAPI support with Produces, Name and ExcludeFromDescription attributes
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
