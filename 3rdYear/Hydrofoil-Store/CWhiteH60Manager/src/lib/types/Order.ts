type Order = {
    orderId: number;
    customerId: number;
    dateCreated: Date;
    dateFulfilled: Date | null;
    total: number;
    taxes: number;
    customer: {
        customerId: number;
        firstName: string | null;
        lastName: string | null;
        email: string;
        phoneNumber: string | null;
        province: string | null;
        creditCard: string | null;
    };
};

export type { Order };
