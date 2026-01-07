# Contributing to CompetencyApp

Thank you for your interest in contributing to CompetencyApp! This document provides guidelines for contributing to our .NET 9 Blazor Server application.

## 🚀 Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- Git
- Code editor (Visual Studio, VS Code, or JetBrains Rider)

### Local Development Setup
1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/your-username/competency-app.git
   cd competency-app
   ```
3. Install dependencies:
   ```bash
   dotnet restore
   ```
4. Build and run:
   ```bash
   dotnet build
   dotnet run
   ```

## 🏗️ Architecture Guidelines

This is a Blazor Server application following these patterns:

### Component Development
- **Async/Await**: All service calls must use async patterns
- **Lifecycle Methods**: Use appropriate Blazor component lifecycle methods
- **Dependency Injection**: Services should be injected, not instantiated
- **Separation of Concerns**: Keep data models, services, and UI separate

### Code Style
- Follow existing Blazor Server patterns
- Use Bootstrap classes for consistent styling
- Maintain the service-based architecture
- Add comprehensive error handling with try-catch blocks

### Adding New Competency Frameworks
To add a new competency type (e.g., DevOps, Product Manager):

1. **Create JSON Configuration**:
   ```bash
   touch wwwroot/data/[TYPE]Competency.json
   ```

2. **Follow Schema Structure**:
   ```json
   {
     "sliders": [
       {
         "id": 1,
         "name": "competency_name",
         "label": "Display Name",
         "minValue": 1,
         "maxValue": 5,
         "defaultValue": 3,
         "category": "Category Name",
         "description": "Brief description",
         "levels": [
           {
             "level": 1,
             "name": "Novice",
             "description": "Level description",
             "characteristics": ["Characteristic 1", "Characteristic 2"]
           }
           // Add levels 2-5
         ]
       }
     ]
   }
   ```

3. **Update Display Names** (Optional):
   ```csharp
   // In Services/SliderService.cs
   public string GetCompetencyDisplayName(string competencyType)
   {
       return competencyType switch
       {
           "SE" => "Software Engineer",
           "QA" => "QA Engineer",
           "NEW" => "Your New Type", // Add here
           _ => competencyType
       };
   }
   ```

## 🔄 Development Workflow

### Branch Naming
- Feature branches: `feature/description`
- Bug fixes: `fix/description`
- Documentation: `docs/description`

### Pull Request Process
1. **Create Feature Branch**:
   ```bash
   git checkout -b feature/new-competency-framework
   ```

2. **Make Changes**: Follow the coding guidelines above

3. **Test Your Changes**:
   ```bash
   dotnet build
   dotnet run
   # Test all competency types work
   # Test radar charts render properly
   # Test local storage functionality
   ```

4. **Commit with Descriptive Message**:
   ```bash
   git commit -m "feat: add DevOps competency framework with 12 competencies"
   ```

5. **Push and Create PR**:
   ```bash
   git push origin feature/new-competency-framework
   ```

### Pull Request Requirements
- [ ] Code follows existing Blazor Server patterns
- [ ] All async operations use async/await
- [ ] Bootstrap styling is maintained
- [ ] New competency frameworks follow Dreyfus model (5 levels)
- [ ] Local storage keys are competency-specific
- [ ] Documentation is updated for new features
- [ ] Application builds without errors
- [ ] All competency types work correctly

## 🐛 Bug Reports

When reporting bugs, include:
- **Description**: Clear description of the issue
- **Steps to Reproduce**: Detailed steps to recreate the problem
- **Expected Behavior**: What should happen
- **Actual Behavior**: What actually happens
- **Environment**: .NET version, browser, operating system
- **Console Errors**: Any JavaScript or server errors

## 💡 Feature Requests

For new features:
- **Use Case**: Describe the problem you're trying to solve
- **Proposed Solution**: Your suggested approach
- **Alternatives**: Other solutions you've considered
- **Competency Framework**: If applicable, which frameworks would benefit

## 🧪 Testing

### Manual Testing Checklist
- [ ] All competency frameworks load correctly
- [ ] Sliders save values to local storage
- [ ] Radar charts render for each competency type
- [ ] Notes are saved and persist across sessions
- [ ] Export functionality works
- [ ] Responsive design works on mobile
- [ ] Navigation between competency types works

## 📝 Code of Conduct

### Our Standards
- **Respectful**: Treat all contributors with respect
- **Inclusive**: Welcome diverse perspectives and backgrounds
- **Professional**: Keep discussions focused on the project
- **Helpful**: Assist others in learning and contributing

### Unacceptable Behavior
- Harassment or discriminatory language
- Personal attacks or trolling
- Spam or off-topic discussions
- Publishing private information without permission

## 🏷️ Versioning

We use [Semantic Versioning](https://semver.org/):
- **MAJOR**: Breaking changes to competency framework structure
- **MINOR**: New competency frameworks or significant features
- **PATCH**: Bug fixes and minor improvements

## 📞 Getting Help

- **Issues**: Create a GitHub issue for bugs or feature requests
- **Discussions**: Use GitHub Discussions for questions
- **Documentation**: Check the README.md for setup instructions

## 🙏 Recognition

Contributors will be acknowledged in:
- GitHub contributors list
- Release notes for significant contributions
- README.md acknowledgments section

Thank you for helping make CompetencyApp better for everyone!