# Különböző Git Konfiguráció Munkahelyi és Hobby Projektekhez

## Lépések

### 1. Hozz létre két külön konfigurációs fájlt:
- **Munkahelyi beállítások:** `D:/work/.gitconfig-work`
- **Hobby beállítások:** `D:/home/.gitconfig-home`

### 2. Szerkeszd a globális `.gitconfig` fájlt:
A globális `.gitconfig` fájl általában a következő helyen található:
- Linux/Mac: `~/.gitconfig`
- Windows: `C:/Users/<felhasználó>/.gitconfig`

Add hozzá a következő beállításokat:

```
[includeIf "gitdir:D:/work/"]
    path = D:/work/.gitconfig-work
[includeIf "gitdir:D:/home/"]
    path = D:/home/.gitconfig-home
```


### 3. Példa a konfigurációs fájlok tartalmára:

#### Munkahelyi beállítások (`D:/work/.gitconfig-work`):

```
[user] 
    name = Munkahelyi Nevem
    email = munka@example.com
[core]
    autocrlf = true
```


#### Hobby beállítások (`D:/home/.gitconfig-home`):
```
[user]
    name = Hobby Nevem
    email = hobby@example.com
[pull]
    rebase = true
```


### 4. Ellenőrzés

#### Munkahelyi projekt:
Nyiss egy terminált a `D:/work/project1` mappában, és futtasd:


```bash
git config --get user.email
```

**Eredmény:** `munka@example.com`

#### Hobby projekt:
Nyiss egy terminált a `D:/home/project2` mappában, és futtasd:

```bash
git config --get user.email
```

**Eredmény:** `hobby@example.com`

### 5. Megjegyzések
- A `gitdir:` kulcsszó a mappa elérési útját jelöli, ahol a Git projekt található.
- A `path` kulcsszó a konfigurációs fájl elérési útját jelöli.
- A `includeIf` kulcsszó lehetővé teszi, hogy a Git csak akkor töltse be a megadott konfigurációs fájlt, ha a megadott feltétel teljesül.

 Ha úgy tűnik, hogy a Git globális konfigurációs fájlban (`~/.gitconfig` vagy `C:\Users\<felhasználó>\.gitconfig`) található beállítások nem érvényesülnek, akkor néhány dolgot érdemes ellenőrizni:
 - **Git verzió:** Győződj meg róla, hogy a Git legfrissebb verzióját használod, mivel a `includeIf` funkció csak a 2.13-as verziótól érhető el.
 - Ellenőrizd, hogy a `.gitconfig` fájl a helyes helyen van-e. Windows-on ez általában `C:\Users\<felhasználó>\.gitconfig`.
 Futtasd ezt a parancsot a repository-dban, hogy lásd az összes beállítást és forrásukat:
 ```
 git config --global --list --show-origin
 ```
 Ez kiírja az összes globális beállítást és azt is, hogy melyik fájlból olvasta őket.
 Ha a Git nem találja a globális `.gitconfig` fájlt (vagy nem ott ahol várnád), annak oka lehet, hogy a rendszered környezeti változói, például a `HOMEDRIVE` és a `HOMEPATH`, megváltoztak (például IT csoport által beállított csoportházirend miatt). A Git a `HOMEDRIVE` és `HOMEPATH` kombinációját használja, csak akkor, ha a `HOME` változó nincs definiálva.
 Megoldás: Definiáld a `HOME` környezeti változót manuálisan, például:
 ```bash
    setx HOME "C:\Users\<felhasználó>"
 ```
 Újraindítást igényelhet, hogy a változások érvénybe lépjenek!
