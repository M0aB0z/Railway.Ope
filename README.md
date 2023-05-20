# Introduction 
Railway.Ope is a light .NET solution to use *Railway Programming* approach.

The purpose of this concept is to:

- Prevent application instabilities
- Keep as much as technical context as possible when an error occured
- Have a finest control than global exception handler on your methods
- Wrap a readable user message to expose to your end user 

# Getting Started
Restore dependancies with command 
`
dotnet restore
`
# Basic usage

All your methods should now return an Ope an 

```cs
 public Ope<bool> IsContractVisibleForUser(int contractId, User user) {
    try {
      return Ope.Ok(contractId % 2 == 0); // some business logic here..
    }
    catch(Exception er) {
      return Ope.Error<bool>(er, "An error occured while checking contract visibility").Tag(contractId).Tag(user.Id);
    }
 }

  public Ope<Contract> GetContractForUser(int contractId, User user) 
  {
    Contract res = null;
    try {
        var visibilityOpe = IsContractVisibleForUser(contractId, user);
        if(!visibilityOpe.Success) {
            return Ope.Error(res, visibilityOpe); // Will keep error context and return a Ope<Contract>
        }
        res = GetContractDetails(contractId, userDetailsOpe.Result);
    } catch(Exception err) {
        return Ope.Error(res, err, "An error occured while retrieving contract details").Tag(contractId).Tag(user.Id);
    }
    return Ope.Ok(res);
  }

```

# Advanced usage

You can also chain multiple actions in a simple way:

```cs
  public Task<float> Multiply(float a, float b) => Task.FromResult(a * b);

  [Test]
  public void ShouldRunSuccessOperation()
  {
      Ope<string> chainedOpeResult = Ope.Run(() => Divide(100, 3), "Maths operation fails") // 300
          .Then(res => res + 22)   // 322
          .Then(res => res.ToString()); // "322"

      Check.That(chainedOpeResult.Success).IsTrue();
      Check.That(chainedOpeResult.Result).Equals("322");
  }

```

# ILogger extension

You can easily extend your ILogger interface with the following extension helper:

````cs
  public static class LoggerExtensions
  {
      public static void LogError(this ILogger logger, OpeBase ope) => logger.LogError(ope.ToString());
  }
````

# Sample log output

test code:
````cs
  int userId = 1357;
  var userRoles = new[] { "IT_ADMIN", "READER" };
  var opeError = Ope.Error("User details retrievement fails").Tag(userId).Tag(userRoles);

  logger.LogError(opeError);

````
Log output:
````
Operation fails - [User details retrievement fails]

|> [userId] -> [1357]
|> [userRoles] -> [IT_ADMIN, READER]
````

# Web app Usage

On your top level API controller it may be interresting to:

-  return Ok(ope.Result) on success
-  log ope.ToString() on files & Sentry, then return BadRequest(ope.UserMessage) on error & handle it in a     global http interceptor on your frontEnd

Enjoy
