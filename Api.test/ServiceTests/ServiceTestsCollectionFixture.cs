namespace Api.test.ServiceTests;

[CollectionDefinition("ServiceTests")]
public class ServiceTestCollection
    : ICollectionFixture<ServiceTestFixture>,
        ICollectionFixture<JwtFixture> { }
