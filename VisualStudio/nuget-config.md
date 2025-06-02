# NuGet Configuration Guide

This guide provides detailed instructions on how to globally configure NuGet settings, including adding package sources and credentials. It covers manual editing of `nuget.config` files, Visual Studio integration, and includes examples for Telerik and GitHub Packages.

---

## 📁 Types of `nuget.config` Files

NuGet supports hierarchical configuration, and config files are read in the following order (lowest to highest priority):

1. **Machine-wide config** (applies to all users and solutions)
2. **User-specific config** (global config for the current user)
3. **Solution-level config** (located in the solution folder)
4. **Project-level config** (near the project file)

> The **closest file to the project** overrides higher-level settings.

---

## 🌍 Global `nuget.config` File

### Default location

- **Windows:**  
  `%appdata%\NuGet\nuget.config`
  
- **Linux/macOS:**  
  `~/.config/NuGet/NuGet.Config`

### Finding it from Command Line

```bash
nuget config -list
```

This will display all loaded `nuget.config` files and their paths in order of precedence.

---

## 🔧 Adding Package Sources

You can add a package source either manually or via Visual Studio.

### Example: Telerik and GitHub Package Sources

#### 1. Manual Configuration (via `nuget.config`)

> ⚠️ **Important:** In XML, you must escape special characters:  
> `&` → `&amp;`  
> `<` → `&lt;`  
> `>` → `&gt;`  
> `'` → `&apos;`  
> `"` → `&quot;`

```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="Telerik" value="https://nuget.telerik.com/nuget" />
    <add key="GitHub" value="https://nuget.pkg.github.com/your-org-name/index.json" />
  </packageSources>

  <packageSourceCredentials>
    <Telerik>
      <add key="Username" value="your-telerik-username" />
      <add key="ClearTextPassword" value="your-telerik-password" />
    </Telerik>
    <GitHub>
      <add key="Username" value="your-github-username" />
      <add key="ClearTextPassword" value="your-personal-access-token" />
    </GitHub>
  </packageSourceCredentials>
</configuration>
```

#### 2. Visual Studio Configuration

1. Open Visual Studio.
2. Navigate to **Tools → NuGet Package Manager → Package Manager Settings**.
3. Under **NuGet Package Manager**, select **Package Sources**.
4. Click the **+** button to add a new source.
5. Enter a **Name** and **Source URL** (e.g., Telerik or GitHub URL).
6. Use the **gear icon** to enter credentials when prompted (encrypted and saved securely).

---

## 🧪 Example: Solution-Level `nuget.config`

Place a `nuget.config` file in your solution directory if you want to override or extend sources for a specific project.

Example structure:

```
MySolution/
├── nuget.config
├── Project1/
└── Project2/
```

This is useful for CI/CD scenarios or per-repository configuration.

---

## 📌 Tips & Best Practices

- Avoid committing sensitive credentials in plaintext. Prefer environment variables or encrypted secrets for CI/CD.
- If using GitHub Actions or other pipelines, pass credentials securely via secrets.
- Use the `dotnet nuget add source` CLI for scripting:

```bash
dotnet nuget add source https://nuget.telerik.com/nuget \
  --name Telerik \
  --username your-username \
  --password your-password \
  --store-password-in-clear-text
```

- Use `--configfile` option to specify a custom `nuget.config`:

```bash
dotnet restore --configfile ./nuget.config
```

---

## 🔍 Debugging Configuration Issues

- Run restore with verbose logging to debug issues:

```bash
dotnet restore --verbosity detailed
```

- Use `nuget sources list` to verify currently configured sources:

```bash
nuget sources list
```

---

## 📚 Further Reading

- [NuGet Configuration Docs](https://learn.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior)
- [Package Source Credential Providers](https://learn.microsoft.com/en-us/nuget/reference/extensibility/nuget-credential-providers)

---
