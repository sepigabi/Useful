
# 📊 SQL Server Express vs. Web vs. Standard – Funkció-összehasonlítás

| Funkció / Tulajdonság                         | Express                                | Web                                      | Standard                                        |
|----------------------------------------------|----------------------------------------|------------------------------------------|------------------------------------------------|
| **Célközönség**                               | Fejlesztés / kisalkalmazás             | Webes alkalmazások backendje             | KKV-k, belső alkalmazások                      |
| **Licenc**                                    | Ingyenes                               | SPLA (hostolt környezet)                 | Kereskedelmi                                   |
| **Max DB méret**                              | 10 GB / adatbázis                      | Nincs korlát (csak RAM/CPU lim.)        | Nincs korlát                                   |
| **Max memória (buffer pool)**                 | 1 GB                                   | ~64 GB (verziófüggő)                     | 128 GB                                         |
| **Max. CPU használat**                        | 1 socket / 4 core                      | 4 socket / 16 core                       | 24 core / 4 socket                             |
| **Database Engine**                           | ✅                                     | ✅                                       | ✅                                             |
| **SQL Server Agent**                          | ❌                                     | ✅                                       | ✅                                             |
| **Full-Text Search**                          | ✅ (külön választható)                 | ✅                                       | ✅                                             |
| **Replikáció (Subscriber)**                   | ❌                                     | ✅ (csak subscriber)                     | ✅ (minden szerepben)                          |
| **Backup Compression**                        | ❌                                     | ❌                                       | ✅                                             |
| **Partitioning**                              | ❌                                     | ❌                                       | ✅                                             |
| **Encryption (TDE, Always Encrypted)**        | ❌                                     | ❌                                       | ✅                                             |
| **Advanced security (RLS, auditing)**         | ❌                                     | ❌                                       | ✅ (korlátozott)                               |
| **Change Data Capture / CDC**                 | ❌                                     | ❌                                       | ✅                                             |
| **SQL Server Reporting Services (SSRS)**      | ❌                                     | ⚠️ (csak alap szinten)                 | ✅                                             |
| **Integration Services (SSIS)**               | ❌                                     | ❌                                       | ✅                                             |
| **Analysis Services (SSAS)**                  | ❌                                     | ❌                                       | ✅                                             |
| **High Availability (Failover / Always On)**  | ❌                                     | ❌                                       | ✅ (két csomópontos cluster)                   |
| **Machine Learning / AI (R/Python)**          | ❌                                     | ❌                                       | ❌ (csak Enterprise)                           |

---

## 📌 Ajánlott használati esetek

| Edition   | Használati példa                                                      |
|-----------|------------------------------------------------------------------------|
| Express   | Fejlesztői környezet, kis eszközök, lokális alkalmazás prototípus     |
| Web       | Weboldal, API backend, hosztolt alkalmazás harmadik félnek            |
| Standard  | Vállalati üzleti rendszerek, belső használatra, BI / SSIS / SSRS-hez  |

---

## 🧾 Összefoglalás

- **Express**: teljesen ingyenes, de erősen korlátozott (max. 10 GB/adatbázis, nincs Agent).
- **Web**: licencelhető SPLA alatt, ideális hostolt webes backend szolgáltatásokhoz.
- **Standard**: általános célú vállalati megoldás, gazdagabb funkciók, magasabb teljesítmény.
