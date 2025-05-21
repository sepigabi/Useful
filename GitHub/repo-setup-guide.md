# Git Repository Setup and Workflow Guide

This guide combines best practices for setting up a new GitHub repository and working with it locally, along with project structure suggestions and useful Git commands.

---

## 📦 Create a New GitHub Repository

1. Go to [GitHub](https://github.com) and create a new repository.
2. ✅ Make sure to check the options:
   - Add `.gitignore`
   - Add `README.md`

---

## 💻 Clone the Repository Locally

```bash
git clone <remote repository URL>
```

> Replace `<remote repository URL>` with your actual repository link.

You can also initialize an existing project directory with:

```bash
git init -b main
git remote add origin <remote repository URL>
git pull origin main
```

If you encounter an unrelated history error:

```bash
git pull origin main --allow-unrelated-histories
```

---

## 🗂️ Suggested Project Structure

Include `.gitkeep` in empty folders to keep them in the repo.

```
/user-preferences-service
│── /src                     
│   ├── /server                      # Backend + Admin UI
│   │   ├── /UserPreferences.Core    # NuGet package (logic, EF models, etc.)
│   │   ├── /UserPreferences.Api     # API backend (Web API)
│   │   ├── /UserPreferences.AdminUI # Admin interface (Blazor/Razor Pages)
│   │   ├── /tests  
│   │   ├── server.sln                
│   ├── /client                     # Frontend + NPM packages
│   │   ├── /user-preferences-sdk   # Library published as NPM package
│   │   ├── /demo-app               # Demo Angular/Vue/React app
│── /docs
│   ├── /architecture.md
│   ├── /development-setup.md
│   ├── /changelog.md
│   ├── /user-guide.md
│   ├── /openapi.yaml
│   ├── /changelog.md           
│── /scripts     
│── /tools                  
│── /.github
│   ├── /workflows
│   │   ├── /build.yaml
│   │   ├── /release.yaml          
│── README.md
```

---

## 🔧 Git Configuration

If not already set globally or locally, configure your Git identity:

```bash
git config user.name "Gabor Sepsei"
git config user.email "gabor.sepsei@binarit.hu"
```

To check current settings:

```bash
git config user.name
git config user.email
```

Enable rebase during pull (optional):

```bash
git config pull.rebase true
```

---

## 🧼 Handling .gitignore Changes

If `.gitignore` was added after files were committed:

```bash
git rm -r --cached .
git add .
git commit -am "Remove ignored files and resubmit files"
git push origin main
```

---

## 🔁 Rename Default Branch (if needed)

If you want to rename `master` to `main`:

```bash
git branch -m master main
```

More info: [Git Tower Guide](https://www.git-tower.com/learn/git/faq/git-rename-master-to-main/)

---

## 🚀 First Commit Workflow

```bash
git pull origin main
git add --all
git commit -m "First commit"
git push origin main
```

---

## ⚙️ GitHub Actions

Add GitHub Actions workflows in the `.github/workflows` directory with custom `.yaml` files based on your CI/CD needs.

