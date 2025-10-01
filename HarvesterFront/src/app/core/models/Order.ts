import { Combine } from './Combine';
import { OrderStatus } from './enums/order-status';
import { StrawProcessingMethod } from './enums/straw-processing-method';
import { Field } from './Field';

export interface Order {
  id: number;
  field: Field;
  combine: Combine;
  orderDate: Date;
  scheduledDate: Date;
  status: OrderStatus;
  strawProcessingMethod: StrawProcessingMethod;
  estimatedTime: number;
  estimatedPrice: number;
  totalPrice: number;
  isArchived: boolean;
}
