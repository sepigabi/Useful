# Angular Getting Started Guide

A step-by-step guide for creating, structuring, and managing a modern Angular project using best practices, standalone APIs, and optional tools like NgRx, PrimeNG, or ngx-translate.

---

## 🛠 Initial Setup

To create a new Angular application, you can use the Angular CLI:

### Basic project creation:

```bash
ng new MyApp
```

### Recommended option (to avoid embedded Git repo):

```bash
ng new MyApp --skip-git
```

### Full example with extra options:

```bash
ng new MyApp --skip-git --style=scss --ssr=false
```

> ✅ `--skip-git` is recommended if you're creating the project inside an existing Git repository (e.g., a monorepo).  
> ❌ Otherwise, you may end up with an **embedded Git repository**, which can cause conflicts and version control issues.

---

## 📦 Install Required Packages

Once inside the project folder (`cd MyApp`), you can install useful packages:

### ✴ NgRx Signals

```bash
ng add @ngrx/signals
ng add @ngrx/operators
```

### 🎨 PrimeNG UI Kit

```bash
npm install primeng primeicons
```

Add the styles to your `angular.json`:

```json
"styles": [
  "node_modules/primeicons/primeicons.css",
  "node_modules/primeng/resources/themes/lara-light-blue/theme.css",
  "node_modules/primeng/resources/primeng.min.css",
  "src/styles.scss"
]
```

### 🌐 ngx-translate

```bash
npm install @ngx-translate/core @ngx-translate/http-loader
```

Basic setup inside your `app.config.ts` or `app.module.ts` for using translations.

---

## 🌱 Don't Forget: Setup Environments

Angular supports environment-specific configuration files for handling differences between development, staging, and production.

➡ To learn how to add and use custom environments properly, see:  
[📄 add-environments.md](./add-environments.md)

This step is important for managing:
- Different API URLs
- Feature toggles
- Logging levels

---

## 🗂️ Suggested Project Structure

A clean, modular folder structure helps to scale Angular projects efficiently. Below is a recommended structure for a modern Angular application using **standalone APIs**, **feature-first design**, and **clear separation of concerns**.

```
src/
├── app/
│   ├── services/             → App-wide singleton services (AuthService, Logger, ErrorHandler)
│   ├── state/                → Global application state management (e.g. @ngrx stores, signal stores)
│   ├── shared/               → UI-only reusable elements (no services allowed here!)
│   │   ├── components/       → Generic presentational components (buttons, cards, etc.)
│   │   ├── pipes/            → Global pipes (e.g. date, truncate)
│   │   ├── directives/       → Global structural or attribute directives
│   ├── utils/                → Pure utility logic (not Angular-specific)
│   │   ├── validators/       → Custom validators used across forms
│   │   ├── helpers/          → Other generic helper functions (e.g. string utils, math helpers)
│   ├── model/                → Global data models and types
│   │   ├── api/              → Interfaces representing backend API contracts (DTOs)
│   │   ├── enums/            → Shared enums across features
│   │   ├── types/            → Shared TypeScript types and interfaces
│   ├── features/             → Business domains grouped by feature
│   │   ├── reports/
│   │   │   ├── components/   → Feature-specific presentational components
│   │   │   ├── services/     → Feature-specific services (e.g. ReportsApiService)
│   │   │   ├── store/        → Local (Component) store or Signal store for this feature
│   │   ├── dashboard/        → Similar structure for the dashboard feature
│   │   └── users/            → Similar structure for the users feature
│   ├── app.routes.ts         → Global routing setup using `provideRouter`
│   ├── app.config.ts         → Application-wide providers and configuration (ApplicationConfig)
│   ├── app.component.ts      → Root component
│   └── app.component.html    → Root template
├── assets/                   → Static files used in the app
│   ├── images/               → Application-wide images (e.g. logos, illustrations)
│   ├── icons/                → Icon sets or SVGs
│   └── translations/         → i18n translation JSON files
├── environments/             → Environment configs (environment.ts, environment.prod.ts)
└── main.ts                   → Entry point with `bootstrapApplication()`
```
### 🔍 Notes

- ✅ **Standalone-first:** This structure is optimized for Angular’s standalone components and modern bootstrapping.
- 🧼 **Separation of concerns:** Business logic is in `services/`, UI elements in `shared/`, models in `model/`.
- 🔁 **Reusable and feature-oriented:** `shared/` for common elements, `features/` for domain separation.

✅ This layout keeps things maintainable, discoverable, and scalable — ready for both small and enterprise-grade Angular apps.

---

## ✍️ Write Your Code
Implement your aplication logic in:

- Use `shared/` for reusable components and pipes
- Use `features/` for domain-specific logic
- Use `services/`, `state/`, and `model/` for app-wide concerns

---

## 🐞 Debug

To run the app locally:

```bash
ng serve
```

Then open `http://localhost:4200` in your browser.

> 💡 You can also use `--open` to launch it automatically:  
> `ng serve --open`

---

## 🧪 Test

To run unit tests:

```bash
ng test
```

This uses **Karma** by default (you can configure alternatives like Jest).

---

## 🏗 Build

To build a production-ready bundle:

```bash
ng build
```

This creates the output in the `dist/` folder, optimized for deployment.

---

## 🚀 Publish (Deploy) Angular Application

This section covers how to deploy your **Angular application** (not a library) to two common targets:

1. **IIS (Internet Information Services)**
2. **GitHub Pages**

Each target includes both **manual** and **automated** (CI/CD via GitHub Actions) deployment approaches.

---

### 🌐 1. Deploy to IIS

#### 🧰 Manual Deployment

1. Build the app:

```bash
ng build --configuration production
```

2. Copy the contents of `dist/<your-app-name>/` to your IIS server's web root folder (e.g., `C:\inetpub\wwwroot\MyApp`).

3. Ensure `web.config` is present (use Angular’s default if needed). Example for routing support:

```xml
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

#### 🤖 CI/CD with GitHub Actions (to IIS via FTP)

```yaml
name: Deploy to IIS

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      - name: Install Node.js
        uses: actions/setup-node@v4
        with:
          node-version: 20

      - name: Install dependencies
        run: npm install

      - name: Build Angular App
        run: npm run build -- --configuration production

      - name: FTP Deploy to IIS
        uses: SamKirkland/FTP-Deploy-Action@v4
        with:
          server: ${{ secrets.FTP_SERVER }}
          username: ${{ secrets.FTP_USERNAME }}
          password: ${{ secrets.FTP_PASSWORD }}
          local-dir: dist/your-app-name/
```

> 🔐 Store your FTP credentials in **GitHub Secrets**.

---

### 🌍 2. Deploy to GitHub Pages

#### 🧰 Manual Deployment

1. Build the app with the correct `base-href`:

```bash
ng build --configuration production --base-href /<repo-name>/
```

2. Install Angular CLI GitHub Pages deploy tool:

```bash
npm install -g angular-cli-ghpages
```

3. Deploy manually:

```bash
npx angular-cli-ghpages --dir=dist/<your-app-name>
```

#### 🤖 CI/CD with GitHub Actions (gh-pages branch)

```yaml
name: Deploy to GitHub Pages

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: 20

      - name: Install dependencies
        run: npm install

      - name: Build Angular app
        run: npm run build -- --configuration production --base-href /<repo-name>/

      - name: Deploy to GitHub Pages
        uses: JamesIves/github-pages-deploy-action@v4
        with:
          branch: gh-pages
          folder: dist/<your-app-name>
```

> 🧠 Ensure GitHub Pages is enabled for the `gh-pages` branch in your repository settings.

✅ Now you're ready to publish your Angular **application** to either **IIS** or **GitHub Pages** – manually or via **GitHub Actions CI/CD**!

---

✅ You're now ready to create professional-grade Angular applications using scalable architecture, reusable modules, and modern build workflows.
