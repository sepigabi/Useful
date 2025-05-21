# GitHub Workflow Variables

The syntax for referencing variables in a GitHub Actions workflow depends on the context in which the variable is used.

## 1. `${{ env.NugetSourceUrl }}`

- **Usage:** When referencing environment variables in the input value of a step or action within the workflow YAML.
- **Syntax:** GitHub Actions YAML expression syntax.
- **Common Use:** In the `with` block of an action or when interpolating variables in YAML expressions.

**Example:**
```yaml
with:
  source-url: ${{ env.NugetSourceUrl }}
```

---

## 2. `$NugetSourceUrl`

- **Usage:** When referencing environment variables in a shell script or command executed as part of a `run` step.
- **Syntax:** Standard shell syntax for accessing environment variables.

**Example:**
```yaml
run: echo $NugetSourceUrl
```

---

## 3. `${NugetSourceUrl}`

- **Usage:** In shell scripts when you need to explicitly delimit the variable name to avoid ambiguity in concatenated strings.
- **Syntax:** Shell syntax with explicit variable name delimitation.

**Example:**
```yaml
run: echo "The NuGet source is ${NugetSourceUrl} for today"
```

---

## 🔑 Key Differences

| Syntax                   | Context               | Description |
|--------------------------|-----------------------|-------------|
| `${{ ... }}`             | GitHub Actions YAML   | Used to evaluate expressions within the workflow YAML. |
| `$VAR` / `${VAR}`        | Shell script, `run:`  | Shell-specific syntax. Use `${VAR}` for clarity or in complex strings. |

---

## 🧾 Summary

- **In workflow YAML:** ` ${{ env.VAR }} `
- **In shell commands (`run:`):** `$VAR` or `${VAR}`

