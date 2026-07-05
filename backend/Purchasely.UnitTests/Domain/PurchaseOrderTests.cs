using FluentAssertions;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.UnitTests.Domain;

public class PurchaseOrderTests
{
    private static PurchaseOrder CreateIssuedPO(List<PurchaseOrderLine>? lines = null)
    {
        lines ??= [CreateLine(quantityOrdered: 10)];

        var po = PurchaseOrder.Create(
            supplierId: Guid.NewGuid(),
            requisitionId: Guid.NewGuid(),
            createdBy: Guid.NewGuid(),
            lines: lines,
            taxRate: 0);

        po.Issue();
        return po;
    }

    private static PurchaseOrderLine CreateLine(decimal quantityOrdered = 10) =>
        PurchaseOrderLine.Create(
            productId: Guid.NewGuid(),
            quantityOrdered: quantityOrdered,
            unitPrice: 50);


    public class RecordReceipt
    {
        [Fact]
        public void Should_set_status_to_received_when_all_lines_fulfilled()
        {
            var line = CreateLine(quantityOrdered: 10);
            var po = CreateIssuedPO([line]);

            po.RecordReceipt([(line.Id, 10)]);

            po.Status.Should().Be(PurchaseOrderStatus.Received);
        }

        [Fact]
        public void Should_set_status_to_partially_received_when_not_all_fulfilled()
        {
            var line = CreateLine(quantityOrdered: 10);
            var po = CreateIssuedPO([line]);

            po.RecordReceipt([(line.Id, 5)]);

            po.Status.Should().Be(PurchaseOrderStatus.PartiallyReceived);
        }

        [Fact]
        public void Should_accumulate_quantity_across_multiple_receipts()
        {
            var line = CreateLine(quantityOrdered: 10);
            var po = CreateIssuedPO([line]);

            po.RecordReceipt([(line.Id, 4)]);
            po.RecordReceipt([(line.Id, 6)]);

            po.Status.Should().Be(PurchaseOrderStatus.Received);
            po.Lines.First().QuantityReceived.Should().Be(10);
        }

        [Fact]
        public void Should_set_status_to_received_when_all_lines_fulfilled_across_multiple_deliveries()
        {
            var line1 = CreateLine(quantityOrdered: 10);
            var line2 = CreateLine(quantityOrdered: 5);
            line2.Id = Guid.NewGuid();
            var po = CreateIssuedPO([line1, line2]);

            po.RecordReceipt([(line1.Id, 10)]);
            po.Status.Should().Be(PurchaseOrderStatus.PartiallyReceived);

            po.RecordReceipt([(line2.Id, 5)]);
            po.Status.Should().Be(PurchaseOrderStatus.Received);
        }
    }


    public class Totals
    {
        [Fact]
        public void Should_calculate_subtotal_correctly()
        {
            var lines = new List<PurchaseOrderLine>
            {
                PurchaseOrderLine.Create(Guid.NewGuid(), 2, 100),
                PurchaseOrderLine.Create(Guid.NewGuid(), 3, 50)
            };

            var po = PurchaseOrder.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), lines, taxRate: 0);

            po.SubTotal.Should().Be(350);
        }

        [Fact]
        public void Should_calculate_tax_and_total_correctly()
        {
            var lines = new List<PurchaseOrderLine>
            {
                PurchaseOrderLine.Create(Guid.NewGuid(), 10, 100)
            };

            var po = PurchaseOrder.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), lines, taxRate: 0.1m);

            po.SubTotal.Should().Be(1000);
            po.TaxAmount.Should().Be(100);
            po.TotalAmount.Should().Be(1100);
        }
    }

    public class CanTransitionTo
    {
        [Theory]
        [InlineData(PurchaseOrderStatus.Draft, PurchaseOrderStatus.Issued, true)]
        [InlineData(PurchaseOrderStatus.Draft, PurchaseOrderStatus.Cancelled, true)]
        [InlineData(PurchaseOrderStatus.Draft, PurchaseOrderStatus.Received, false)]
        [InlineData(PurchaseOrderStatus.Issued, PurchaseOrderStatus.PartiallyReceived, true)]
        [InlineData(PurchaseOrderStatus.Issued, PurchaseOrderStatus.Received, true)]
        [InlineData(PurchaseOrderStatus.Issued, PurchaseOrderStatus.Cancelled, true)]
        [InlineData(PurchaseOrderStatus.Issued, PurchaseOrderStatus.Draft, false)]
        [InlineData(PurchaseOrderStatus.PartiallyReceived, PurchaseOrderStatus.Received, true)]
        [InlineData(PurchaseOrderStatus.PartiallyReceived, PurchaseOrderStatus.Cancelled, false)]
        [InlineData(PurchaseOrderStatus.Received, PurchaseOrderStatus.Closed, true)]
        [InlineData(PurchaseOrderStatus.Received, PurchaseOrderStatus.Cancelled, false)]
        [InlineData(PurchaseOrderStatus.Closed, PurchaseOrderStatus.Received, false)]
        [InlineData(PurchaseOrderStatus.Cancelled, PurchaseOrderStatus.Issued, false)]
        public void Should_validate_transition(
            PurchaseOrderStatus from,
            PurchaseOrderStatus to,
            bool expected)
        {
            var po = PurchaseOrder.Create(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                [CreateLine()], taxRate: 0);

            typeof(PurchaseOrder)
                .GetProperty(nameof(PurchaseOrder.Status))!
                .SetValue(po, from);

            po.CanTransitionTo(to).Should().Be(expected);
        }
    }
}