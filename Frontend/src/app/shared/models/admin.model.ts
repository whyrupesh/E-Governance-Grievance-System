export interface Department {
  id: number;
  name: string;
}

export interface Category {
  id: number;
  name: string;
  departmentId: number;
}

export interface Officer {
  id: number;
  fullName: string;
  email: string;
  department: string;
}
