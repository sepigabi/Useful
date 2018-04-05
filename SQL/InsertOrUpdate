UPDATE MyTable SET FieldA=@FieldA WHERE Key=@Key

IF @@ROWCOUNT = 0
   INSERT INTO MyTable (FieldA) VALUES (@FieldA)
