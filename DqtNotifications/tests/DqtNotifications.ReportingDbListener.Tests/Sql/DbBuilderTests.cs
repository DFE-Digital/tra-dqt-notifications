using System.Collections.Immutable;
using Dapper;
using DqtNotifications.ReportingDbListener.Sql;
using Microsoft.Extensions.Logging.Abstractions;
using static DqtNotifications.ReportingDbListener.Sql.DbBuilder;

namespace DqtNotifications.ReportingDbListener.Tests.Sql;

public class DbBuilderTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public DbBuilderTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task InsertRow_RowDoesNotExist_InsertsRowAndReturnsSuccess()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilder = new DbBuilder(conn, clock, new NullLogger<DbBuilder>());

        var tableName = "contact";
        var id = Guid.NewGuid();
        var version = 1L;
        var firstName = "Joe";
        var middleName = (string?)null;
        var lastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", firstName)
            .Add("MiddleName", middleName)
            .Add("LastName", lastName);

        var result = await dbBuilder.InsertRow(tableName, id, version, columnValues, CancellationToken.None);
        Assert.Equal(InsertRowResult.Success, result);

        var rows = await _fixture.Query("select * from contact where id = @Id", new { Id = id });

        Assert.Collection(
            rows,
            row =>
            {
                Assert.Equal(id, row["Id"]);
                Assert.Equal(clock.UtcNow, row["SinkCreatedOn"]);
                Assert.Equal(clock.UtcNow, row["SinkModifiedOn"]);
                Assert.Equal(version, row["versionnumber"]);
                Assert.Equal(firstName, row["firstname"]);
                Assert.Equal(middleName, row["middlename"]);
                Assert.Equal(lastName, row["lastname"]);
            });
    }

    [Fact]
    public async Task InsertRow_RowAlreadyExists_ReturnsRowAlreadyExists()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilder = new DbBuilder(conn, clock, new NullLogger<DbBuilder>());

        var tableName = "contact";
        var id = Guid.NewGuid();
        var version = 1L;
        var firstName = "Joe";
        var middleName = (string?)null;
        var lastName = "Bloggs";

        await conn.ExecuteAsync("insert into contact (Id) values (@id)", new { id });

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", firstName)
            .Add("MiddleName", middleName)
            .Add("LastName", lastName);

        var result = await dbBuilder.InsertRow(tableName, id, version, columnValues, CancellationToken.None);
        Assert.Equal(InsertRowResult.RowAlreadyExists, result);
    }

    [Theory]
    [InlineData(2L)]
    [InlineData(1L)]
    public async Task UpdateRow_RowExistsWithEarlierOrEqualRowVersion_UpdatesRowAndReturnsSuccess(long newVersion)
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilder = new DbBuilder(conn, clock, new NullLogger<DbBuilder>());

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 1L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName);

        var result = await dbBuilder.UpdateRow(tableName, id, newVersion, columnValues, CancellationToken.None);
        Assert.Equal(UpdateRowResult.Success, result);

        var rows = await _fixture.Query("select * from contact where id = @Id", new { Id = id });

        Assert.Collection(
            rows,
            row =>
            {
                Assert.Equal(id, row["Id"]);
                Assert.Equal(createdOn, row["SinkCreatedOn"]);
                Assert.Equal(clock.UtcNow, row["SinkModifiedOn"]);
                Assert.Equal(newVersion, row["versionnumber"]);
                Assert.Equal(newFirstName, row["firstname"]);
                Assert.Equal(newMiddleName, row["middlename"]);
                Assert.Equal(newLastName, row["lastname"]);
            });
    }

    [Fact]
    public async Task UpdateRow_RowDoesNotExists_ReturnsRowDoesNotExist()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilder = new DbBuilder(conn, clock, new NullLogger<DbBuilder>());

        var tableName = "contact";
        var id = Guid.NewGuid();

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName);

        var result = await dbBuilder.UpdateRow(tableName, id, newVersion, columnValues, CancellationToken.None);
        Assert.Equal(UpdateRowResult.RowDoesNotExist, result);
    }

    [Fact]
    public async Task UpdateRow_RowExistsWithLaterRowVersion_DoesNotUpdateRowAndReturnsRowVersionStale()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilder = new DbBuilder(conn, clock, new NullLogger<DbBuilder>());

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 3L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName);

        var result = await dbBuilder.UpdateRow(tableName, id, newVersion, columnValues, CancellationToken.None);
        Assert.Equal(UpdateRowResult.RowVersionStale, result);

        var rows = await _fixture.Query("select * from contact where id = @Id", new { Id = id });

        Assert.Collection(
            rows,
            row =>
            {
                Assert.Equal(id, row["Id"]);
                Assert.Equal(createdOn, row["SinkCreatedOn"]);
                Assert.Equal(createdOn, row["SinkModifiedOn"]);
                Assert.Equal(initialVersion, row["versionnumber"]);
                Assert.Equal(initialFirstName, row["firstname"]);
                Assert.Equal(initialMiddleName, row["middlename"]);
                Assert.Equal(initialLastName, row["lastname"]);
            });
    }

    [Fact]
    public async Task UpsertEntityRow_NewRowHintAndRowDoesNotExist_InsertsRowAndReturnsSuccess()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var version = 1L;
        var firstName = "Joe";
        var middleName = (string?)null;
        var lastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", firstName)
            .Add("MiddleName", middleName)
            .Add("LastName", lastName)
            .Add("versionnumber", version);

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.NewRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.Success, result);
        dbBuilderMock.Verify(mock => mock.InsertRow(tableName, id, version, It.IsAny<ImmutableDictionary<string, object?>>(), CancellationToken.None));
    }

    [Fact]
    public async Task UpsertEntityRow_NewRowHintAndRowDoesExistWithEarlierVersion_UpdatesRowAndReturnsSuccess()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 1L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName)
            .Add("versionnumber", newVersion);

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.NewRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.Success, result);
        dbBuilderMock.Verify(mock => mock.InsertRow(tableName, id, newVersion, It.IsAny<ImmutableDictionary<string, object?>>(), CancellationToken.None));
    }

    [Fact]
    public async Task UpsertEntityRow_NewRowHintAndRowDoesExistWithLaterVersion_ReturnsStaleDataError()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 3L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName)
            .Add("versionnumber", newVersion);

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.NewRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.StaleDataError, result);
    }

    [Fact]
    public async Task UpsertEntityRow_NewRowHintAndRowDoesExistWithEarlierVersionButDeletedBeforeUpdate_ReturnsStaleDataError()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 1L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName)
            .Add("versionnumber", newVersion);

        // Delete the row right before UpdateRow runs
        dbBuilderMock
            .Setup(mock => mock.UpdateRow(tableName, id, newVersion, It.IsAny<ImmutableDictionary<string, object?>>(), CancellationToken.None))
            .Callback(() => conn.Execute("delete from contact where Id = @id", new { id }));

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.NewRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.StaleDataError, result);
    }

    [Fact]
    public async Task UpsertEntityRow_ExistingRowHintAndRowExistsWithEarlierVersionNumber_UpdatesRowAndReturnsSuccess()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 1L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName)
            .Add("versionnumber", newVersion);

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.ExistingRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.Success, result);
        dbBuilderMock.Verify(mock => mock.UpdateRow(tableName, id, newVersion, It.IsAny<ImmutableDictionary<string, object?>>(), CancellationToken.None));
    }

    [Fact]
    public async Task UpsertEntityRow_ExistingRowHintAndRowDoesNotExist_ReturnsStaleDataError()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName)
            .Add("versionnumber", newVersion);

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.ExistingRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.StaleDataError, result);
    }

    [Fact]
    public async Task UpsertEntityRow_ExistingRowHintAndRowExistsWithLaterVersionNumber_ReturnsStaleDataError()
    {
        using var conn = await _fixture.GetConnection();
        var clock = new TestClock();
        var dbBuilderMock = new Mock<DbBuilder>(conn, clock, new NullLogger<DbBuilder>()) { CallBase = true };

        var tableName = "contact";
        var id = Guid.NewGuid();
        var initialVersion = 3L;
        var createdOn = clock.UtcNow.Subtract(TimeSpan.FromHours(1));
        var initialFirstName = Faker.Name.First();
        var initialMiddleName = Faker.Name.Middle();
        var initialLastName = Faker.Name.Last();

        await conn.ExecuteAsync(
            "insert into contact (Id, SinkCreatedOn, SinkModifiedOn, versionnumber, firstname, middlename, lastname) " +
            "values (@id, @createdOn, @createdOn, @initialVersion, @initialFirstName, @initialMiddleName, @initialLastName)",
            new { Id = id, createdOn, initialVersion, initialFirstName, initialMiddleName, initialLastName });

        var newVersion = 2L;
        var newFirstName = "Joe";
        var newMiddleName = (string?)null;
        var newLastName = "Bloggs";

        var columnValues = ImmutableDictionary.Create<string, object?>()
            .Add("FirstName", newFirstName)
            .Add("MiddleName", newMiddleName)
            .Add("LastName", newLastName)
            .Add("versionnumber", newVersion);

        var result = await dbBuilderMock.Object.UpsertEntityRow(tableName, id, RowStateHint.ExistingRow, columnValues, CancellationToken.None);
        Assert.Equal(UpsertEntityRowResult.StaleDataError, result);
    }

    [Fact]
    public void MapAttributesToColumns_MapsEntityReferenceToTwoColumns()
    {
        var ownerId = Guid.NewGuid();
        var ownerEntityLogicalName = "systemuser";

        var entity = EntityDelta.Create(
            "contact",
            Guid.NewGuid(),
            new Dictionary<string, object?>()
            {
                {
                    "owner",
                    new EntityReference(ownerEntityLogicalName, ownerId)
                }
            });

        var columnValues = DbBuilder.MapAttributesToColumns(entity);

        Assert.Contains(columnValues, kvp => kvp.Key == "owner" && kvp.Value?.Equals(ownerId) == true);
        Assert.Contains(columnValues, kvp => kvp.Key == "owner_entitytype" && kvp.Value?.Equals(ownerEntityLogicalName) == true);
    }
}
