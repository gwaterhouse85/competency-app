# CompetencyApp - Professional Competency Assessment Platform

A comprehensive Blazor Server Application for assessing and tracking professional competencies across multiple engineering disciplines using the proven Dreyfus Model of skill acquisition.

## 🚀 Features

- **Multi-Framework Support**: Software Engineer (SE) and QA Engineer (QA) competency frameworks with extensible architecture
- **Dreyfus Model Integration**: 5-level skill assessment (Novice → Advanced Beginner → Competent → Proficient → Expert)
- **Interactive Radar Charts**: Visual representation of competency profiles using HTML5 Canvas
- **AI-Powered Suggestions**: Get personalized improvement recommendations using Azure OpenAI
- **Local Storage Persistence**: Client-side data storage with separate namespaces per competency type
- **Real-time Feedback**: Dynamic level descriptions and characteristics as you adjust ratings
- **Personal Notes System**: Document experiences, goals, and learning resources for each competency
- **Export Functionality**: Download complete assessments as structured JSON files
- **Responsive Design**: Bootstrap-powered interface that works on desktop and mobile

## 🏗️ Application Architecture

### Project Structure

```
CompetencyApp/
├── Data/                          # Data models and entities
│   └── SliderModel.cs            # Core competency and level models
├── Services/                      # Business logic and data services
│   ├── SliderService.cs          # Competency data loading and management
│   └── AzureOpenAIService.cs     # AI-powered suggestion generation
├── Pages/                         # Razor pages and components
│   ├── Index.razor               # Homepage with framework overview
│   ├── Sliders.razor             # Main competency assessment interface
│   ├── CompetencyRadar.razor     # Radar chart visualization
│   └── _Host.cshtml              # Application host page
├── Shared/                        # Shared UI components
│   ├── MainLayout.razor          # Application layout wrapper
│   └── NavMenu.razor             # Dynamic navigation with competency detection
├── wwwroot/                       # Static web assets
│   ├── data/                     # Competency framework definitions
│   │   ├── SECompetency.json     # Software Engineer framework
│   │   └── QACompetency.json     # QA Engineer framework
│   ├── js/                       # Client-side JavaScript
│   │   └── localStorage.js       # Local storage helpers and radar chart rendering
│   └── css/                      # Styling assets (Bootstrap + custom)
└── Program.cs                     # Application entry point and DI configuration
```

### Key Architectural Patterns

- **Service-Based Architecture**: Business logic separated into injectable services
- **Component Lifecycle Management**: Proper use of `OnInitializedAsync`, `OnParametersSetAsync`, and `OnAfterRenderAsync`
- **Dependency Injection**: Services registered and injected following ASP.NET Core patterns
- **Async/Await Throughout**: All data operations use async patterns for performance
- **Bootstrap Styling**: Consistent UI with responsive Bootstrap components

## 🔧 Technology Stack

- **.NET 9.0**: Target framework
- **Blazor Server**: Interactive web UI framework
- **Azure OpenAI**: AI-powered competency improvement suggestions
- **Bootstrap 5**: CSS framework for responsive design
- **HTML5 Canvas**: Radar chart rendering
- **JSON Configuration**: File-based competency framework definitions
- **Browser LocalStorage**: Client-side data persistence

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- Web browser with JavaScript enabled
- Code editor (Visual Studio, VS Code, or JetBrains Rider recommended)
- Azure OpenAI instance (for AI suggestions feature)

## 🚀 Getting Started

### 1. Clone and Setup

```bash
git clone <repository-url>
cd CompetencyApp
dotnet restore
```

### 2. Configure Azure OpenAI

To enable AI-powered suggestions, configure your Azure OpenAI credentials:

**Option A: Using appsettings.Development.json (Recommended for local development)**

Create or update `appsettings.Development.json` with your Azure OpenAI settings:

```json
{
  "AzureOpenAI": {
    "Endpoint": "https://your-instance.openai.azure.com/",
    "ApiKey": "your-api-key",
    "DeploymentName": "your-deployment-name"
  }
}
```

> ⚠️ **Note**: `appsettings.Development.json` is included in `.gitignore` to prevent accidental commits of sensitive data.

**Option B: Using Environment Variables (Recommended for production)**

```bash
export AzureOpenAI__Endpoint="https://your-instance.openai.azure.com/"
export AzureOpenAI__ApiKey="your-api-key"
export AzureOpenAI__DeploymentName="your-deployment-name"
```

### 3. Build the Application

```bash
dotnet build
```

### 4. Run the Development Server

```bash
dotnet run
```

### 5. Access the Application

1. Navigate to the homepage to see available competency frameworks
2. Click on any framework to start an assessment
3. Use the navigation menu to switch between assessments and radar charts
4. On the Radar Chart page, click "Get AI Suggestions" for personalized improvement recommendations

## 🎯 Usage Guide

### Conducting Assessments

1. **Select Framework**: Choose Software Engineer or QA Engineer from the navigation
2. **Rate Skills**: Use the sliders to rate yourself on the 1-5 Dreyfus scale
3. **Add Notes**: Document specific examples and learning goals
4. **View Progress**: Check the competency summary for overall progress

### Visualizing Results

1. **Radar Charts**: Navigate to the radar chart for visual representation
2. **Growth Areas**: Review recommendations for skill development
3. **Export Data**: Download assessments for portfolio or review purposes

### Data Management

- **Automatic Saving**: All changes are saved to browser local storage immediately
- **Framework Isolation**: Each competency type maintains separate data
- **Export Options**: JSON exports include full assessment data with timestamps

## 🔧 Development

### Adding New Competency Frameworks

1. **Create JSON Configuration**:
   ```bash
   # Example: Project Manager competency
   touch wwwroot/data/PMCompetency.json
   ```

2. **Follow Schema Structure**:
   ```json
   {
     "sliders": [
       {
         "id": 1,
         "name": "project_planning",
         "label": "Project Planning",
         "minValue": 1,
         "maxValue": 5,
         "defaultValue": 3,
         "category": "Planning & Strategy",
         "description": "Project planning and execution",
         "levels": [
           {
             "level": 1,
             "name": "Novice",
             "description": "Basic project planning understanding",
             "characteristics": ["Follows templates", "Needs guidance"]
           }
           // ... additional levels
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
           "PM" => "Project Manager", // Add new mapping
           _ => competencyType
       };
   }
   ```

4. **Test**: The framework will be automatically detected and appear in navigation

### Code Standards

- **Async Operations**: All service calls must use `async/await`
- **Component Lifecycle**: Use appropriate lifecycle methods for data loading
- **Bootstrap Classes**: Use Bootstrap utilities for consistent styling
- **Error Handling**: Include try-catch blocks with meaningful error messages
- **Local Storage**: Use competency-specific keys for data isolation


## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details on:

- How to set up your development environment
- Coding standards and Blazor Server patterns
- Adding new competency frameworks
- Submitting pull requests
- Reporting bugs and requesting features

### Quick Start for Contributors

1. Fork the repository
2. Clone your fork and create a feature branch
3. Follow our [Blazor Server coding guidelines](CONTRIBUTING.md#architecture-guidelines)
4. Ensure all async operations use async/await patterns
5. Test your changes across all competency frameworks
6. Submit a pull request with a clear description

## 🐛 Bug Reports & Feature Requests

- **Bug Reports**: Use our [bug report template](.github/ISSUE_TEMPLATE/bug_report.md)
- **Feature Requests**: Use our [feature request template](.github/ISSUE_TEMPLATE/feature_request.md)
- **Security Issues**: See our [Security Policy](SECURITY.md)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### What this means:
- ✅ **Commercial use** - Use in commercial projects
- ✅ **Modification** - Modify the source code
- ✅ **Distribution** - Distribute the software
- ✅ **Private use** - Use for private purposes
- ⚠️ **Liability** - Software is provided "as-is"
- ⚠️ **Warranty** - No warranty is provided

## 🙏 Contributing & Community

### Ways to Contribute
- 🐛 **Report bugs** using our issue templates
- 💡 **Suggest features** for new competency frameworks
- 🔧 **Submit pull requests** following our guidelines
- 📖 **Improve documentation** 
- 🎨 **Enhance UI/UX** while maintaining Bootstrap consistency
- 📊 **Add competency frameworks** for new roles (DevOps, Product Manager, etc.)

### Community Guidelines
- Follow our [Code of Conduct](CONTRIBUTING.md#code-of-conduct)
- Be respectful and inclusive
- Help others learn and contribute
- Keep discussions focused and professional

## 📊 Competency Frameworks

These are merely included to demonstrate how the app can work with different competencies and specialisms and are in no way how we believe competencies should be. 

### Software Engineer (SE)
**21 Competencies across 6 Categories:**
- **Frontend Development**: Frameworks, HTML/CSS, JavaScript/TypeScript, UI/UX
- **Backend Development**: Server Languages, APIs, Databases, Security
- **DevOps & Infrastructure**: Containers, CI/CD, Cloud, Monitoring
- **Testing & Quality**: Unit Testing, Integration Testing, Code Review
- **Tools & Practices**: Git, Agile, Documentation
- **Soft Skills**: Problem Solving, Communication, Leadership

### QA Engineer (QA)
**8 Competencies across 4 Categories:**
- **Test Strategy & Planning**: Test Planning, Manual Testing
- **Automation & Tools**: Test Automation, API Testing
- **Specialized Testing**: Performance Testing, Security Testing
- **Process & Quality**: Defect Management, Agile QA

## 🔧 Configuration

### Environment Variables
```bash
# Development
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:5001;http://localhost:5000

# Production
ASPNETCORE_ENVIRONMENT=Production
```

### Azure OpenAI Configuration

The AI suggestions feature requires an Azure OpenAI instance. Configure the following settings:

| Setting | Description | Example |
|---------|-------------|--------|
| `Enabled` | Enable/disable AI suggestions feature | `true` or `false` |
| `Endpoint` | Your Azure OpenAI resource endpoint | `https://your-instance.openai.azure.com/` |
| `ApiKey` | API key from Azure Portal | `abc123...` |
| `DeploymentName` | Name of your deployed model | `gpt-35-turbo-1106` |

> **Note**: Set `Enabled` to `false` in `appsettings.json` (default) and `true` in `appsettings.Development.json` to only enable AI features when credentials are configured.

**Supported Models**: The application is tested with GPT-3.5 Turbo and GPT-4 deployments.

### Application Settings
Configuration is managed through `appsettings.json` and `appsettings.Development.json`.

- `appsettings.json` - Contains placeholder values, safe to commit
- `appsettings.Development.json` - Contains your actual credentials, excluded from git

## 🚀 Deployment

## 🆘 Support

For questions and support:
1. Check existing issues in the repository
2. Create a new issue with detailed information
3. Follow the issue template for faster resolution

---

Built with ❤️ using .NET 9 Blazor Server and the Dreyfus Model of skill acquisition.

**Open Source** • **MIT Licensed** • **Community Driven**
