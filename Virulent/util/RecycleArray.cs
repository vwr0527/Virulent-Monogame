using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

/**
 * This is a list of generic objects that are created and die often.
 * To avoid excessive garbage build up, we do not "delete" elements
 * that have died. We simply mark them as deleted. Then, when a new
 * element is spawned, we re-incarnate the element as a new element.
 * Since a type T may or may not have a method that copies all it's
 * members into a new T, We use a Action<> specified by the user to
 * copy a newly spawned object into the list.
 */
namespace Virulent
{
    //remember to reset the size (by clearing the list) after a level
    class RecycleArray<T>
    {
        List<Cell<T>> cellList;
        int max_index = 0;
        int num_active = 0;
        int current_index = 0;
        Action<T, T> CopyMembers;
        Func<T, T> CreateCopyMethod;                //only need this because you can't create a new T
        bool set_data_instead_of_copy = false;

        public RecycleArray(Action<T,T> copyMethod, Func<T, T> createCopyMethod)
        {
            CopyMembers = copyMethod;
            CreateCopyMethod = createCopyMethod;
            cellList = new List<Cell<T>>();
        }
        public RecycleArray(int size, Action<T, T> copyMethod, Func<T, T> createCopyMethod)
        {
            CopyMembers = copyMethod;
            CreateCopyMethod = createCopyMethod;
            cellList = new List<Cell<T>>(size);
            max_index = size - 1;
        }
        public RecycleArray(IEnumerable<Cell<T>> collection, Action<T, T> copyMethod, Func<T, T> createCopyMethod)
        {
            CopyMembers = copyMethod;
            CreateCopyMethod = createCopyMethod;
            cellList = new List<Cell<T>>(collection);
            max_index = collection.Count() - 1;
        }
        //==================================================

        // if it's full, add to the list.
        // if it's not full, When an empty cell is found,
        // clone the added element into it.
        // afterwards, current index is the newly added element
        // ADDENDUM: Returns the actual thing that was added or created.
        public T Add(T data)
        {
            //It's full. extend the list.
            if (num_active == max_index)
            {
                //create and add a new cell to the cell list.
                Cell<T> cell = new Cell<T>();
                //used to be just SetData here. If you used copy data mode, it would create an interesting bug.
                if (set_data_instead_of_copy)
                    cell.SetData(data);
                else
                    cell.CreateCopy(data, CreateCopyMethod);
                cell.Activate();
                cellList.Add(cell);
                ++max_index;
                ++num_active;
                current_index = max_index;

                return cell.GetData();
            }

            //it's not full. There are empty cells. Find them and fill them.
            if (num_active < max_index)
            {
                for (int i = 0; i < max_index; ++i)
                {
                    if (cellList[i].IsActive() == false)
                    {
                        current_index = i;
                        if (set_data_instead_of_copy)
                            cellList[i].SetData(data);
                        else
                            cellList[i].CopyData(data, CopyMembers);
                        cellList[i].Activate();
                        ++num_active;

                        return cellList[i].GetData();
                    }
                }
            }
            //if reached here, add failed
            //TODO: Add error
            System.Console.WriteLine("RecycleArray Add failed");
            return default(T);
        }

        public void EmptyAll()
        {
            for (int i = 0; i < max_index; ++i)
            {
                cellList[i].Deactivate();
            }
            num_active = 0;
            current_index = 0;
        }

        public void DeleteAll()
        {
            cellList.Clear();
            cellList.TrimExcess();
            num_active = 0;
            current_index = 0;
        }

        public void EmptyElementAt(int index)
        {
            if (cellList[index].IsActive())
            {
                cellList[index].Deactivate();
                --num_active;
            }
        }

        public void EmptyElement(T data)
        {
            for (int i = 0; i < max_index; ++i)
            {
                if (cellList[i].IsActive() && cellList[i].GetData().Equals(data))
                {
                    cellList[i].Deactivate();
                    --num_active;
                    return;
                }
            }
        }

        public T ElementAt(int index)
        {
            return cellList[index].GetData();
        }

        public int Size()
        {
            return num_active;
        }

        public int Capacity()
        {
            return max_index;
        }

        public T GetEmptyElement()
        {
            T result = default(T);
            for (int i = 0; i < max_index; ++i)
            {
                if (!cellList[i].IsActive())
                {
                    result = cellList[i].GetData();
                    break;
                }
            }
            return result;
        }

        //If the data is being stored somewhere else, overwriting the data
        //won't cause garbage collection, because it won't delete the only
        //reference to the old data. It will only be copying references.
        public void SetDataMode(bool activate_set_mode)
        {
            set_data_instead_of_copy = activate_set_mode;
        }

        public void Debug()
        {
            System.Console.WriteLine(num_active);
            for (int i = 0; i < max_index; ++i)
            {
                if (cellList[i].IsActive())
                {
                    T result = cellList[i].GetData();
                    System.Console.WriteLine(result);
                }
            }
        }
    }
}
