namespace BaseTypes.NET.Tests
{
    public class EntityTests
    {
        [Fact]
        public void Entities_with_same_id_should_be_equal()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            entity1.Should().Be(entity2);
            (entity1 == entity2).Should().BeTrue();
        }

        [Fact]
        public void Entities_with_different_id_should_not_be_equal()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            entity1.Should().NotBe(entity2);
            (entity1 != entity2).Should().BeTrue();
        }

        private class TestEntity : Entity<Guid>
        {
            public TestEntity(Guid id) : base(id) { }
        }
    }
}