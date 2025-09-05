import { OrderStatus } from './enums/order-status';
import { StrawProcessingMethod } from './enums/straw-processing-method';

export interface Order {
  id: number;
  fieldId: number;
  combineId: number;
  orderDate: Date;
  scheduledDate: Date;
  status: OrderStatus;
  strawProcessingMethod: StrawProcessingMethod;
  estimatedTime: number;
  estimatedPrice: number;
  totalPrice: number;
}
