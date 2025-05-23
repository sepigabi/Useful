# Configurable Angular Library: `user-preferences`

This guide describes how to structure an Angular library (like `user-preferences`) to support both **static and dynamic configuration** using **Standalone APIs** and **Provider Factories**.

The example focuses on a `user-preferences` library, which is used to persist and retrieve a signed-in user's preferences such as language, dark mode, etc. These preferences are stored on a server and can be accessed across devices.

---

## 🧩 Configuration Needs

The `user-preferences` library requires the following configuration:

| Property  | Type       | Description                                                  |
|-----------|------------|--------------------------------------------------------------|
| `url`     | Static     | The server endpoint for storing/retrieving preferences       |
| `userId`  | Dynamic    | The currently signed-in user's ID                            |
| `tenantId`| Static     | The ID of the tenant/environment (multi-tenant support)      |

---

## 🏗 Provider Factory Pattern

Use a provider factory to configure the library at bootstrap time. This enables consumers to pass different configurations while keeping DI setup clean and modular.

### Example

```ts
export abstract class UserPreferencesConfig {
  abstract url: string;
  abstract tenantId: string;
  abstract getUserId: () => string;
}

export const defaultConfig: UserPreferencesConfig = {
  url: '/api/preferences',
  tenantId: 'default',
  getUserId: () => '', // placeholder, must be overridden
};

export function provideUserPreferences(
  config: Partial<UserPreferencesConfig>
): EnvironmentProviders {
  const merged = { ...defaultConfig, ...config };

  return makeEnvironmentProviders([
    {
      provide: UserPreferencesConfig,
      useValue: merged,
    },
  ]);
}
```

### Consuming the Configuration in Services

```ts
@Injectable()
export class UserPreferencesService {
  private config = inject(UserPreferencesConfig);

  savePreference(key: string, value: any) {
    const userId = this.config.getUserId();
    const url = `${this.config.url}?userId=${userId}&tenantId=${this.config.tenantId}`;
    return this.http.post(url, { key, value });
  }
}
```

---

## 🔌 Usage in Application Bootstrap

```ts
bootstrapApplication(AppComponent, {
  providers: [
    provideUserPreferences({
      url: '/api/user-prefs',
      tenantId: 'company-123',
      getUserId: () => authService.currentUser.id,
    }),
  ],
});
```

The `getUserId()` is a **factory function** that allows dynamic resolution of the logged-in user's ID.

---

## 🔌 Usage in `ApplicationConfig` Object

If your application defines an `ApplicationConfig` (commonly in `app.config.ts`), you can integrate the `user-preferences` library configuration directly into the `providers` array:

### `app.config.ts`

```ts
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideUserPreferences } from 'sg/user-preferences';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideUserPreferences({
      url: '/api/user-preferences',
      tenantId: 'acme-corp',
      getUserId: () => localStorage.getItem('currentUserId') || '',
    }),
  ],
};
```

> ✅ This method keeps your library configuration modular and clean, fully leveraging Angular's standalone architecture.

---

## ✅ Benefits

- ✔ **Static & dynamic configuration support**
- ✔ **Clean DI-compatible setup**
- ✔ **Tree-shakable and standalone friendly**
- ✔ Works seamlessly with **bootstrapApplication()**

---

📖 **Source & Inspiration:**  
[Angular Standalone APIs & Provider Patterns – Manfred Steyer, Angular Architects](https://www.angulararchitects.io/blog/patterns-for-custom-standalone-apis-in-angular/)

