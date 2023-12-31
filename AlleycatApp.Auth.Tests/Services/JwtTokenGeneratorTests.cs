using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AlleycatApp.Auth.Infrastructure.Configuration;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace AlleycatApp.Auth.Tests.Services
{
    public class JwtTokenGeneratorTests
    {
        [Fact]
        public void CanGenerateValidToken()
        {
            // Arrange

            var jwtConfig = new JwtConfiguration("audience", "issuer", 15, "11112222333344445555666677778888");
            var appConfigBuilderMock = new Mock<IApplicationConfigurationBuilder>();
            appConfigBuilderMock.Setup(b => b.BuildJwtConfiguration()).Returns(appConfigBuilderMock.Object);
            appConfigBuilderMock.Setup(b => b.Build())
                .Returns(new ApplicationConfiguration { JwtConfiguration = jwtConfig });

            var tokenGenerator = new JwtTokenGenerator(appConfigBuilderMock.Object);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "123"),
                new(ClaimTypes.Name, "user1"),
                new(ClaimTypes.Role, "Role")
            };

            // Act

            var token = (JwtSecurityToken)tokenGenerator.GenerateToken(claims);

            // Assert

            Assert.Equal(jwtConfig.Issuer, token.Issuer);
            Assert.Equal(jwtConfig.Audience, token.Audiences.Single());
            Assert.Contains(("nameid", "123"), token.Claims.Select(c => (c.Type, c.Value)));
            Assert.Contains(("unique_name", "user1"), token.Claims.Select(c => (c.Type, c.Value)));
            Assert.Contains(("role", "Role"), token.Claims.Select(c => (c.Type, c.Value)));
            Assert.Equal(SecurityAlgorithms.HmacSha256, token.SignatureAlgorithm);
        }

        [Fact]
        public void CanSerializeToken()
        {
            // Arrange

            var jwtConfig = new JwtConfiguration("audience", "issuer", 15, "11112222333344445555666677778888");
            var appConfigBuilderMock = new Mock<IApplicationConfigurationBuilder>();
            appConfigBuilderMock.Setup(b => b.BuildJwtConfiguration()).Returns(appConfigBuilderMock.Object);
            appConfigBuilderMock.Setup(b => b.Build())
                .Returns(new ApplicationConfiguration { JwtConfiguration = jwtConfig });

            var tokenGenerator = new JwtTokenGenerator(appConfigBuilderMock.Object);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "123"),
                new(ClaimTypes.Name, "user1"),
                new(ClaimTypes.Role, "Role")
            };

            // Act

            var token = (JwtSecurityToken)tokenGenerator.GenerateToken(claims);
            var serializedToken = tokenGenerator.SerializeToken(token);
            var deserializedToken = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(serializedToken);

            // Assert

            Assert.Equal(token.Id, deserializedToken.Id);
            Assert.Equal(token.Issuer, deserializedToken.Issuer);
            Assert.Equal(token.SecurityKey, deserializedToken.SecurityKey);
            Assert.Equal(token.ValidFrom, deserializedToken.ValidFrom);
            Assert.Equal(token.ValidTo, deserializedToken.ValidTo);
            Assert.Equal(token.Audiences, deserializedToken.Audiences);
            Assert.Equal(token.Claims.Select(c => (c.Type, c.Value)), deserializedToken.Claims.Select(c => (c.Type, c.Value)));
        }
    }
}
