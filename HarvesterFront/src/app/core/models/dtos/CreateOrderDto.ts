import { StrawProcessingMethod } from '../enums/StrawProcessingMethod';

export interface CreateOrderDto {
  fieldId: number;
  combineId: number;
  orderDate: Date;
  strawProcessingMethod: StrawProcessingMethod;
}
