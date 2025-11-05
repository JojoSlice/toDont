namespace Api.test.ServiceTests;

public class InvalidIdsTestData : TheoryData<int>
{
    public InvalidIdsTestData()
    {
        Add(-1);
        Add(0);
        Add(999);
        Add(int.MaxValue);
    }
}

public class InvalidUserNamesTestData : TheoryData<string>
{
    public InvalidUserNamesTestData()
    {
        Add("nonexistentuser");
        Add("");
        Add("user_that_does_not_exist");
    }
}
