using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Tests.Models
{
    public class RaceModelTests
    {
        [Fact]
        public void ValidModelPassesValidation()
        {
            // Arrange

            var race1 = new Race
            {
                Id = 1,
                BeginTime = new DateTime(2023, 12, 5),
                Description = "Sample description",
                IsActive = true,
                IsFreeOrder = false,
                Name = "Race 1",
                StartAddress = "Sample address 1",
                ValueModifier = 3.4M
            };

            var race2 = new Race
            {
                Id = 1,
                BeginTime = new DateTime(2023, 12, 5),
                IsActive = true,
                IsFreeOrder = false,
                Name = "Race 2",
                StartAddress = "Sample address 2",
            };

            var errors = new List<ValidationResult>();
            var context1 = new ValidationContext(race1);
            var context2 = new ValidationContext(race2);

            // Act

            var result1 = Validator.TryValidateObject(race1, context1, errors, true);
            var result2 = Validator.TryValidateObject(race2, context2, errors, true);

            // Assert

            Assert.True(result1);
            Assert.True(result2);
            Assert.Empty(errors);
        }

        [Fact]
        public void CannotValidateModelWithExcessivelyLongFields()
        {
            // Arrange

            var longName1 = "";
            var longName2 = "";

            for (var i = 0; i < 65; i++) longName1 += "A";
            for (var i = 0; i < 257; i++) longName2 += "A";

            var race1 = new Race
            {
                Id = 1,
                BeginTime = new DateTime(2023, 12, 5),
                Description = longName2,
                IsActive = true,
                IsFreeOrder = false,
                Name = "Race 1",
                StartAddress = "Sample address",
                ValueModifier = 3.4M
            };

            var race2 = new Race
            {
                Id = 1,
                BeginTime = new DateTime(2023, 12, 5),
                Description = "Sample description",
                IsActive = true,
                IsFreeOrder = false,
                Name = longName1,
                StartAddress = "Sample address",
                ValueModifier = 3.4M
            };

            var race3 = new Race
            {
                Id = 1,
                BeginTime = new DateTime(2023, 12, 5),
                Description = "Sample description",
                IsActive = true,
                IsFreeOrder = false,
                Name = "Race 1",
                StartAddress = longName2,
                ValueModifier = 3.4M
            };

            var errors = new List<ValidationResult>();
            var context1 = new ValidationContext(race1);
            var context2 = new ValidationContext(race2);
            var context3 = new ValidationContext(race3);

            // Act

            var result1 = Validator.TryValidateObject(race1, context1, errors, true);
            var result2 = Validator.TryValidateObject(race2, context2, errors, true);
            var result3 = Validator.TryValidateObject(race3, context3, errors, true);

            // Assert

            Assert.False(result1);
            Assert.False(result2);
            Assert.False(result3);

            Assert.Equal(3, errors.Count);
            Assert.Contains(nameof(Race.Name), errors.Select(e => e.MemberNames.Single()));
            Assert.Contains(nameof(Race.Description), errors.Select(e => e.MemberNames.Single()));
            Assert.Contains(nameof(Race.StartAddress), errors.Select(e => e.MemberNames.Single()));
        }
    }
}
