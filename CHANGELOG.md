# Changelog (Release Notes)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html) (SemVer).

## [1.0.3] - 2025-01-26

### Added

- Tests for extension methods

### Changed
- Replace quotes in build yml
- Split dependencies between net461 and another
- Namespaces for extensions according to MS guide

## [1.0.2] - 2025-01-18

### Added

- Make packages AotCompatible from net6.0 and higher

### Changed

- Change some docs in V3 parser

### Removed

- Remove unnecessary null check for message in V3 parser

### Fixed

- Fix infinite wait when PingDelay equals zero
- Fix null exceptions on extensions setupAction

## [1.0.1] - 2025-01-09

### Changed

- Change IrcNet.Client.Extensions.Core from csproj to shproj

### Removed

- IrcNet.Client.Extensions.Core Dependency
- Full build on nuget packing

## [1.0.0] - 2025-01-08

### Added

- Initial release

[1.0.3]: https://github.com/NowaLone/IrcNet/releases/tag/v1.0.3
[1.0.2]: https://github.com/NowaLone/IrcNet/releases/tag/v1.0.2
[1.0.1]: https://github.com/NowaLone/IrcNet/releases/tag/v1.0.1
[1.0.0]: https://github.com/NowaLone/IrcNet/releases/tag/v1.0.0
