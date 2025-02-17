namespace BaseTypes.NET.Tests
{
    public class EnumerationTests
    {
        [Fact]
        public void Enumeration_should_retrieve_by_value()
        {
            var status = Enumeration.GetByValue<OrderStatus>(2);
            status.Should().Be(OrderStatus.Shipped);
        }

        [Fact]
        public void Enumeration_should_retrieve_by_display_name()
        {
            var status = Enumeration.GetByDisplayName<OrderStatus>("Delivered");
            status.Should().Be(OrderStatus.Delivered);
        }

        [Fact]
        public void Enumeration_fields_with_different_values_should_not_be_equal()
        {
            var statusPending = Enumeration.GetByValue<OrderStatus>(1);
            var statusShipped = Enumeration.GetByValue<OrderStatus>(2);
            statusPending.Should().NotBe(statusShipped);
            (statusPending != statusShipped).Should().BeTrue();
        }

        [Fact]
        public void Enumeration_fields_with_same_values_should_be_equal()
        {
            var statusPending = Enumeration.GetByDisplayName<OrderStatus>("Pending");
            var statusPendingDuplicate = Enumeration.GetByDisplayName<OrderStatus>("Pending Duplicate");
            statusPending.Should().Be(statusPendingDuplicate);
            (statusPending == statusPendingDuplicate).Should().BeTrue();
        }

        private class OrderStatus : Enumeration
        {
            public static readonly OrderStatus Pending = new(1, "Pending");
            public static readonly OrderStatus PendingDuplicate = new(1, "Pending Duplicate");
            public static readonly OrderStatus Shipped = new(2, "Shipped");
            public static readonly OrderStatus Delivered = new(3, "Delivered");

            private OrderStatus(int value, string displayName) : base(value, displayName) { }
        }
    }
}
