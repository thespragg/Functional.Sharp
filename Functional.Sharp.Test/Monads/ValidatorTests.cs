using Functional.Sharp.Monads;

namespace Functional.Sharp.Test.Monads;

public class ValidatorTests
{
    [Fact]
    public void Validator_ReturnsSuccessfulValidation()
    {
        var validator = Validator.For<int>().AddRule(x => x is 5, "Not Equal to 5");
        var errors = validator.Validate(5);
        Assert.Empty(errors);
    }

    [Fact]
    public void Validator_ReturnsFailedValidation()
    {
        var validator = Validator.For<int>().AddRule(x => x is 5, "Not Equal to 5");
        var errors = validator.Validate(10).ToArray();
        Assert.Single(errors);
        Assert.Equal("Not Equal to 5", errors.FirstOrDefault());
    }

    [Fact]
    public void Validator_ReturnsReturnsCorrectValidations()
    {
        var validator = Validator.For<int>()
            .AddRule(
                x => x is 5,
                "Not Equal to 5"
            )
            .AddRule(
                x => x < 20,
                "Not Less than 20"
            )
            .AddRule(
                x => x > 5,
                "Not Greater than 5"
            );

        var errors = validator.Validate(5).ToArray();
        Assert.Single(errors);
        Assert.Equal("Not Greater than 5", errors.FirstOrDefault());
    }
}