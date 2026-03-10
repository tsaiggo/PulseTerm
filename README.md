# PulseTerm

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Tests](https://img.shields.io/badge/tests-313%20passing-brightgreen)]()
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)]()
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A modern, cross-platform SSH terminal client built with .NET 10 and Avalonia UI.

## Features

- 🖥️ **Cross-Platform**: Runs on Windows, macOS, and Linux
- 🔐 **Secure SSH Connections**: Support for password and private key authentication
- 📁 **SFTP File Browser**: Integrated file transfer with drag-and-drop support
- 🚇 **SSH Tunneling**: Local and remote port forwarding
- 🎨 **Modern UI**: Clean, responsive interface with light and dark themes
- 🌍 **Internationalization**: English and Simplified Chinese (zh-CN) support
- 📝 **Session Management**: Save and organize connection profiles
- 🔄 **Auto-Updates**: Built-in update mechanism using Velopack
- ⚡ **Reactive Architecture**: Built with ReactiveUI for responsive user experience

## Screenshots

*Screenshots coming soon*

## Installation

### From Release

Download the latest release for your platform:
- **Windows**: `PulseTerm-win-x64.zip`
- **macOS**: `PulseTerm-osx-arm64.zip`
- **Linux**: `PulseTerm-linux-x64.zip`

Extract and run the executable.

### From Source

#### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Git

#### Build Instructions

1. Clone the repository:
```bash
git clone https://github.com/tsaiggo/PulseTerm.git
cd PulseTerm
```

2. Build the solution:
```bash
dotnet build src/PulseTerm.slnx
```

3. Run the application:
```bash
dotnet run --project src/PulseTerm.App
```

#### Platform-Specific Builds

Build scripts are provided for each platform:

```bash
# Windows
./scripts/build-win.sh

# macOS (Apple Silicon)
./scripts/build-mac.sh

# Linux
./scripts/build-linux.sh
```

The built application will be in the `publish/` directory.

## Usage

### Quick Start

1. Launch PulseTerm
2. Click "New Connection" or press `Ctrl+N` (Windows/Linux) or `Cmd+N` (macOS)
3. Enter connection details:
   - **Host**: Server hostname or IP address
   - **Port**: SSH port (default: 22)
   - **Username**: Your username
   - **Authentication**: Choose Password or Private Key
4. Click "Connect"

### Session Management

- **Save Sessions**: Save connection profiles for quick access
- **Groups**: Organize sessions into groups
- **Quick Commands**: Store frequently used commands

### SFTP File Browser

- Click the folder icon in the terminal tab to open the SFTP browser
- Drag and drop files to upload
- Right-click for download, delete, and other operations

### SSH Tunneling

- Navigate to the Tunnels panel
- Add local or remote port forwarding rules
- Start/stop tunnels as needed

## Security Notice

⚠️ **Important**: PulseTerm currently stores passwords and private key passphrases in **plain text** on disk at:
- **Windows**: `%USERPROFILE%\.pulseterm\sessions.json`
- **macOS/Linux**: `~/.pulseterm/sessions.json`

**Recommendations**:
- Use private key authentication when possible
- Set file permissions to restrict access (chmod 600 on Unix systems)
- Be aware that any process with user-level access can read these credentials
- Encryption support is planned for a future release

## Development

### Project Structure

```
PulseTerm/
├── src/
│   ├── PulseTerm.App/           # Avalonia UI application
│   │   ├── Views/               # AXAML view files
│   │   ├── ViewModels/          # ReactiveUI ViewModels
│   │   └── Services/            # App-level services
│   ├── PulseTerm.Core/          # Core business logic
│   │   ├── Models/              # Data models
│   │   ├── Services/            # Core services
│   │   ├── Ssh/                 # SSH connection handling
│   │   ├── Sftp/                # SFTP operations
│   │   ├── Tunnels/             # SSH tunneling
│   │   ├── Data/                # Data persistence
│   │   └── Localization/        # i18n resources
│   └── PulseTerm.Terminal/      # Terminal emulation
└── tests/
    ├── PulseTerm.App.Tests/     # UI/ViewModel tests
    ├── PulseTerm.Core.Tests/    # Core logic tests
    └── PulseTerm.Terminal.Tests/# Terminal tests
```

### Running Tests

```bash
# Run all tests
dotnet test src/PulseTerm.slnx

# Run specific test project
dotnet test tests/PulseTerm.Core.Tests

# With code coverage
dotnet test src/PulseTerm.slnx --collect:"XPlat Code Coverage"
```

### Integration Tests

Some tests require a Docker SSH server:

```bash
# Start test SSH server
docker-compose -f docker-compose.test.yml up -d

# Run integration tests
dotnet test tests/PulseTerm.App.Tests --filter Category=Integration

# Stop test server
docker-compose -f docker-compose.test.yml down
```

### Code Style

The project uses `.editorconfig` for consistent code style:
- **C# Indentation**: 4 spaces
- **JSON/XML/AXAML**: 2 spaces
- **Nullable Reference Types**: Enabled
- **UTF-8 Encoding** with LF line endings

### Architecture

PulseTerm follows a layered architecture:
- **Presentation Layer** (`PulseTerm.App`): Avalonia UI + ReactiveUI ViewModels
- **Business Logic Layer** (`PulseTerm.Core`): SSH/SFTP services, data management
- **Terminal Layer** (`PulseTerm.Terminal`): Terminal emulation bridge

Key patterns:
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Reactive Programming**: ReactiveUI + System.Reactive
- **Async/Await**: Consistent use of ConfigureAwait(false)
- **Interface Abstractions**: Testable wrappers around SSH.NET

## Dependencies

Key dependencies:
- [Avalonia](https://avaloniaui.net/) 11.3.12 - Cross-platform UI framework
- [ReactiveUI](https://www.reactiveui.net/) 23.1.8 - Reactive MVVM framework
- [SSH.NET](https://github.com/sshnet/SSH.NET) 2025.1.0 - SSH/SFTP library
- [AvaloniaTerminal](https://github.com/AvaloniaUI/AvaloniaTerminal) 1.0.0-alpha.7 - Terminal emulator
- [Velopack](https://github.com/velopack/velopack) 0.0.1298 - Auto-update framework

## Known Issues

- **AvaloniaTerminal Alpha**: The terminal emulator is currently in alpha. Custom scrollback buffer and UTF-8 decoding have been implemented as workarounds.
- **Password Storage**: Credentials are stored in plain text (see Security Notice above)

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

### Development Setup

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Guidelines

- Follow existing code style and conventions
- Write tests for new functionality
- Update documentation as needed
- Ensure all tests pass before submitting PR

## Roadmap

- [ ] Credential encryption using OS-level APIs (DPAPI/Keychain/Secret Service)
- [ ] Tab splitting and multiple terminal panes
- [ ] Terminal search functionality
- [ ] Script automation and macros
- [ ] Additional language support (Traditional Chinese, Japanese)
- [ ] Performance profiling and optimization
- [ ] More stable terminal emulator (evaluate alternatives)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [Avalonia](https://avaloniaui.net/)
- SSH implementation using [SSH.NET](https://github.com/sshnet/SSH.NET)
- Inspired by [Termius](https://termius.com/) and [FinalShell](https://www.hostbuf.com/)

## Support

- **Issues**: [GitHub Issues](https://github.com/tsaiggo/PulseTerm/issues)
- **Discussions**: [GitHub Discussions](https://github.com/tsaiggo/PulseTerm/discussions)

---

Made with ❤️ by the PulseTerm team
