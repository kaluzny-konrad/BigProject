export class OrderItem {
    id: number | undefined;
    quantity: number = 0;
    unitPrice: number = 0;
    productId: number | undefined;
    productCategory: string | undefined;
    productSize: string | undefined;
    productTitle: string | undefined;
    productArtist: string | undefined;
    productArtId: string | undefined;
}

export class Order {
    orderId: number | undefined;
    orderDate: Date | undefined;
    orderNumber: string = Math.random().toString(36).substring(2,5) + "order";
    items: OrderItem[] = [];

    get subtotal(): number {
        const result = this.items.reduce(
            (total, val) => {
                return total + (val.unitPrice * val.quantity)
            }, 0);
        return result;
    }
}