SELECT
  name, 
  COALESCE(PARSENAME(base_object_name,4),@@servername) AS serverName, 
  COALESCE(PARSENAME(base_object_name,3),DB_NAME(DB_ID())) AS dbName, 
  COALESCE(PARSENAME(base_object_name,2),SCHEMA_NAME(SCHEMA_ID())) AS schemaName, 
  PARSENAME(base_object_name,1) AS objectName 
FROM sys.synonyms 
ORDER BY serverName,dbName,schemaName,objectName
