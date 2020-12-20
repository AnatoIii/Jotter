import { File } from './file';

export class Note {
    id: string;
    categoryID?: string;
    name: string;
    description: string;
    files: File[];
}