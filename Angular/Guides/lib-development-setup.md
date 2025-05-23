# Angular Library Development Setup: `user-preferences-sdk`

This guide walks you through the creation and setup of an Angular library named `user-preferences` inside a workspace called `user-preferences-sdk`.

---

## 🧭 Initial Setup

1. Open a terminal and **navigate to the folder** where you want the project to be created.
2. Create a new Angular workspace **without an application**:

```bash
ng new user-preferences-sdk --create-application=false --skip-git=true
```

3. Navigate into the newly created workspace:

```bash
cd user-preferences-sdk
```

4. Generate the library:

```bash
ng generate library user-preferences
```

> This creates the library under:  
> `projects/user-preferences/`

---

## 📦 Install Required Packages

Make sure you're in the **root** of the workspace (`user-preferences-sdk`) before installing the dependencies. Examples:

```bash
ng add @ngrx/signals@latest
ng add @ngrx/operators@latest
npm install @microsoft/signalr
```

---

## 🛠 Modify `package.json`

Navigate to `projects/user-preferences/package.json` and:

- Rename the package name appropriately, for example:

```json
{
  "name": "sg/user-preferences",
  ...
}
```

- Copy required peer dependencies from the root `package.json` and add them under `peerDependencies`:

```json
"peerDependencies": {
  "@angular/common": "^19.1.0",
  "@angular/core": "^19.1.0",
  "@microsoft/signalr": "^8.0.7",
  "@ngrx/operators": "^19.0.1",
  "@ngrx/signals": "^19.0.1"
}
```

---

## ✍️ Write Your Code

Implement your library logic in:

```
projects/user-preferences/src/lib/
```

---

## 📤 Export Public APIs

Edit the file:

```
projects/user-preferences/src/public-api.ts
```

And export the relevant modules and services:

```ts
export * from './lib/user-preferences.service';
export * from './lib/user-preferences-signalr.service';
export * from './lib/user-preferences.store';
```

---

## 🏗 Build the Library

From the root of the workspace (`user-preferences-sdk`), build the library:

```bash
ng build user-preferences
```

---

✅ Your Angular library is now set up and ready for use!

---

## 🚀 Publishing to GitHub Packages (NPM)

Publishing your Angular library to GitHub Packages can be done in two ways: manually from your local machine, or automatically via GitHub Actions CI/CD.

---

### 🧱 Prerequisites

Before publishing, ensure your library's `package.json` (at `projects/user-preferences/package.json`) is correctly configured:

```json
{
  "name": "@sepigabi/ngx-user-preferences",
  "version": "0.0.1",
  "publishConfig": {
    "registry": "https://npm.pkg.github.com/"
  },
  "repository": {
    "type": "git",
    "url": "https://github.com/sepigabi/SG.UserPreferences.git"
  },
  "peerDependencies": {
    "@microsoft/signalr": "^8.0.7",
    "@ngrx/operators": "^19.0.1",
    "@ngrx/signals": "^19.0.1"
  }
}
```

> 💡 **Note:** This `package.json` file will be bundled and published from `dist/user-preferences`. The root `package.json` is not used for packaging the library.

---

### 🖐 Manual Publishing (from Local Machine)

#### 🔧 Steps

1. Build the library:

```bash
ng build user-preferences
```

2. Create an `.npmrc` file in the project root:

```ini
//npm.pkg.github.com/:_authToken=YOUR_GITHUB_PERSONAL_ACCESS_TOKEN
@sepigabi:registry=https://npm.pkg.github.com/
```

> Replace `YOUR_GITHUB_PERSONAL_ACCESS_TOKEN` with a [GitHub Personal Access Token](https://github.com/settings/tokens) with `write:packages` and `read:packages` scopes.

3. Navigate to the build output and publish:

```bash
cd dist/user-preferences
npm publish
```

#### 💡 Tips

- Use `npm whoami --registry=https://npm.pkg.github.com/` to verify authentication.
- You must have proper permissions for the package scope in the GitHub repository settings.

---

### 🤖 Automated Publishing (via GitHub Actions)

Use GitHub Actions to publish automatically on version tag pushes.

#### 📁 Example Workflow: `.github/workflows/publish.yml`

```yaml
name: Build and Publish Library

on:
  push:
    tags:
      - 'v*'

jobs:
  build-and-publish:
    runs-on: ubuntu-latest

    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Use Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
          registry-url: 'https://npm.pkg.github.com/'
          scope: '@sepigabi'

      - name: Install dependencies
        run: npm install

      - name: Build library
        run: npm run build user-preferences

      - name: Publish to GitHub Packages
        run: |
          cd dist/user-preferences
          npm publish
        env:
          NODE_AUTH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

#### 💡 Tips

- No `.npmrc` file is needed — GitHub handles auth with `NODE_AUTH_TOKEN`.
- Use a `vX.X.X` tag pattern to trigger publishing automatically.
- Set the correct `scope` and `registry-url` in `setup-node`.

---

✅ With either approach, your Angular library can be published and consumed as a versioned NPM package via GitHub Packages.
