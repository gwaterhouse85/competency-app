# Security Policy

## Supported Versions

We provide security updates for the following versions of CompetencyApp:

| Version | Supported          |
| ------- | ------------------ |
| 1.x.x   | :white_check_mark: |

## Reporting a Vulnerability

If you discover a security vulnerability in CompetencyApp, please report it responsibly:

### Where to Report
- **Email**: Create a GitHub issue with the `security` label
- **Private Disclosure**: For sensitive issues, create a private security advisory

### What to Include
- **Description**: Clear description of the vulnerability
- **Steps to Reproduce**: How to reproduce the issue
- **Impact**: Potential impact of the vulnerability
- **Environment**: .NET version, browser, operating system
- **Suggested Fix**: If you have ideas for fixing the issue

### Security Best Practices

When contributing to CompetencyApp, follow these security guidelines:

#### Data Handling
- All competency data is stored in browser local storage only
- No sensitive personal information is transmitted to servers
- JSON competency frameworks are served as static files

#### Input Validation
- Slider values are validated within min/max ranges
- Text notes are sanitized to prevent XSS
- JSON parsing includes error handling

#### Dependencies
- Keep .NET dependencies up to date
- Bootstrap and other frontend assets are from trusted sources
- Regular dependency security audits
