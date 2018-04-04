catch(DbEntityValidationException e)
{
    StringBuilder errorMessage = new StringBuilder();
    foreach (var error in e.EntityValidationErrors)
    {
        errorMessage.Append($"Entity of type {error.Entry.Entity.GetType().Name} in state {error.Entry.State} has the following validation errors:");
        foreach (var validationError in error.ValidationErrors)
        {
            errorMessage.Append($" - Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
        }
    }
    Trace.WriteLine(new Logger(errorMessage.ToString(), 3 ));
}
