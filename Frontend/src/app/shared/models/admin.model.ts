export interface Department {
  id: number;
  name: string;
  description: string;
  categories: string[];
}

export interface Category {
  id: number;
  name: string;
  departmentId: number;
  departmentName: string;
}

export interface Officer {
  id: number;
  name: string;
  email: string;
  department: string;
  departmentId: number;
}
