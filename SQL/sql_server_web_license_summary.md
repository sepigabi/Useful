
# SQL Server Web Edition licenc megfelelőségi összefoglaló

## 1. Licencforma: SPLA (Service Provider License Agreement)
- Az SQL Server Web Edition kizárólag SPLA keretében érhető el.
- A szervert az Önök cége üzemelteti és licenceli SPLA-n keresztül.
- Ez teljes mértékben megfelel a Web Edition terjesztési és üzemeltetési modelljének.

## 2. Felhasználási modell: Webes szolgáltatás külső cég részére
- A SQL Server egy webes alkalmazást és egy mobilalkalmazást szolgál ki, amelyek:
  - Nem kapcsolódnak közvetlenül az adatbázishoz (nincs közvetlen SQL kapcsolat),
  - Webes API-n keresztül kommunikálnak HTTPS protokollon.
- A végfelhasználók nem az Önök cégének dolgozói, hanem egy másik cég alkalmazottai.
  - Hitelesítéssel érik el az alkalmazást,
  - Interneten keresztül kapcsolódnak a szolgáltatáshoz.

**Ez nem minősül „belső line-of-business (LOB) alkalmazásnak”, hanem „public-facing web service”-nek.**

## 3. Technikai megfelelés
- SSMS csak adminisztrációra használt.
- SQL Reporting Services (SSRS) csak webes felületen keresztül érhető el (ha van).
- Az adatbázist kizárólag a webes backend éri el.
- A mobilalkalmazás zárt terjesztésű, de az API interneten keresztül elérhető.

## 4. Funkcionális és kereskedelmi szándék megfelelés
- A szolgáltatás nem belső használatra készült, hanem egy partnercég számára.
- A Microsoft célja a Web Edition-nel:
> “To support public and Internet-accessible Web pages, sites, applications, and services. It may not be used to support line-of-business applications.”

## Összefoglaló táblázat

| Szempont                         | Megfelel? | Megjegyzés                                          |
|----------------------------------|-----------|------------------------------------------------------|
| SPLA keret                      | ✅         | Web Edition kizárólag SPLA-ban használható           |
| Közvetlen SQL kapcsolat         | ❌         | Nincs – webes API köztes réteg                       |
| Csak belső dolgozók használják? | ❌         | Nem – másik cég dolgozói                             |
| Internetes elérés               | ✅         | API HTTPS-en keresztül elérhető                      |
| Webes alkalmazás jellege        | ✅         | Ügyfél-specifikus, nem LOB                           |
| Admin eszközök (SSMS)           | ✅         | Csak rendszergazdai célra                            |
| Riporting szolgáltatások        | ✅         | Ha van, csak weben keresztül jelenik meg             |

## Végkövetkeztetés
Az Önök rendszerarchitektúrája megfelel a Microsoft SQL Server Web Edition licencfeltételeinek:

- Nem belső LOB alkalmazás,
- Kizárólag interneten keresztül elérhető,
- Közvetetten, webes API-n keresztül kapcsolódik az adatbázishoz,
- Külső ügyfél felhasználásra készült,
- SPLA keretében jogszerűen licencelt.
