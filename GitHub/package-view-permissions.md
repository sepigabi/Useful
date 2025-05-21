# Give View Permissions Per Package, Per Repository

To ensure that your repositories can access specific packages hosted on GitHub Packages, follow the steps below:

## 🔧 Step-by-step Instructions

1. Go to **Packages**
2. Navigate to the package of your library
3. Select **Package settings**
4. In the **Manage Actions Access** section, click on **Add Repository**
5. Add the repository that you want to grant access to this package

---

## ⚠️ Important Note on `GITHUB_TOKEN` Authentication

If you are using `GITHUB_TOKEN` for automatic authentication, you **must explicitly grant view permissions per package and per repository**.

This means that:
- If you have many internal libraries that need to be used in a solution,
- You must manually visit **each package**,
- And grant access to **each repository** that will consume them.

---

## 🧭 Observations

This process can be **surprisingly click-intensive**, especially in large codebases.

> "I certainly hope they improve this process by granting permissions on an organization level for shared libraries, a bulk select, or command line way of granting permissions en masse."

🔗 Source: [StackOverflow – Issue with dotnet restore on GitHub Actions](https://stackoverflow.com/questions/77719331/issue-with-dotnet-restore-on-github-actions-when-accessing-github-packages)
