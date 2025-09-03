import { StrawProcessingMethod } from '../enums/straw-processing-method';

export interface CreateOrderDto {
  fieldId: number;
  combineId: number;
  orderDate: Date;
  strawProcessingMethod: StrawProcessingMethod;
}
