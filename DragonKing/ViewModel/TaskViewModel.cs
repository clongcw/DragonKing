using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonKing.Log.Interface;
using DragonKing.Log.Service;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DragonKing.ViewModel
{
    public partial class TaskViewModel : ObservableObject
    {

        private readonly ILog _log;

        public TaskViewModel(ILog log)
        {
            _log = log;
        }

        #region 方案一，以后待优化
        [RelayCommand]
        async Task Do()
        {
            List<string> values = new List<string> { "甲", "乙", "丙", "丁" };

            // 创建优先级队列，按照任务的优先级从高到低排列
            PriorityQueue<TaskItem> queue = new PriorityQueue<TaskItem>();
            AutoResetEvent StepEvent = new AutoResetEvent(false);

            // 模拟四个人按顺序往队列中添加任务

            await Task.Run(() => EnqueueTasks(queue, values[0]));
            await Task.Run(() => EnqueueTasks(queue, values[1]));
            await Task.Run(() => EnqueueTasks(queue, values[2]));
            await Task.Run(() => EnqueueTasks(queue, values[3]));

            int currentnum = 0;

            // 使用一个LiqD的信号量来控制同时只能有一个LiqD在运行
            SemaphoreSlim liqDSemaphore = new SemaphoreSlim(1,1);

            Task taskD = null;

            while (queue.Count > 0)
            {
                TaskItem taskItem = queue.Dequeue();

                if (taskItem.TaskType == TaskType.Chealing)
                {
                    taskD = Task.Run(async () =>
                    {
                        await liqDSemaphore.WaitAsync();
                        _log.Information($"{taskItem.Person} 正在执行任务：{taskItem.TaskType}");
                        await Task.Delay(10000); // 模拟任务执行的时间
                        _log.Information($"{taskItem.Person} 完成任务：{taskItem.TaskType}");
                        liqDSemaphore.Release();
                        StepEvent.Set();
                        return Task.CompletedTask;
                    });
                }
                else if (taskItem.TaskType == TaskType.Pcr)
                {
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            if (taskD.IsCompleted)
                            {
                                _log.Information($"{taskItem.Person} 正在执行任务：{taskItem.TaskType}");
                                Task.Delay(40000).Wait(); // 模拟任务执行的时间
                                _log.Information($"{taskItem.Person} 完成任务：{taskItem.TaskType}");
                                break;
                            }
                            
                        }
                    });
                }
                else
                {
                    if (liqDSemaphore.CurrentCount >= 1)//LiqD被释放
                    {
                        await ExecuteTask(taskItem);
                    }
                    else if (taskItem.TaskType != TaskType.TransToChip)
                    {
                        await ExecuteTask(taskItem);
                    }
                    else
                    {
                        await ExecuteTask(taskItem);
                        _log.Information($"等待上一个Chealing执行结束再开始执行下一个任务");
                        StepEvent.WaitOne();
                    }


                }
            }
        }


        async Task EnqueueTasks(PriorityQueue<TaskItem> queue, string person)
        {
            // 假设每个人都往队列里添加一些任务
            for (int i = 0; i < 5; i++)
            {
                TaskType taskType = (TaskType)(i % 5); // 循环使用四种任务类型
                TaskItem taskItem = new TaskItem(person, taskType);
                queue.Enqueue(taskItem);
                _log.Information($"{person} 添加了任务：{taskType}");
                await Task.Delay(100); // 模拟任务添加的时间间隔
            }
        }

        async Task ExecuteTask(TaskItem taskItem)
        {
            _log.Information($"{taskItem.Person} 正在执行任务：{taskItem.TaskType}");
            await Task.Delay(1000); // 模拟任务执行的时间
            _log.Information($"{taskItem.Person} 完成任务：{taskItem.TaskType}");
        }

        #endregion

        #region 方案二
        [RelayCommand]
        public async Task Do1()
        {

        }


        #endregion

    }

    public enum TaskType
    {
        AddSmp,
        TransferTemplate,
        TransToChip,
        Chealing,
        Pcr
    }




    class TaskItem : IComparable<TaskItem>
    {
        public string Person { get; }
        public TaskType TaskType { get; }

        public TaskItem(string person, TaskType taskType)
        {
            Person = person;
            TaskType = taskType;
        }

        public int CompareTo(TaskItem other)
        {
            // 优先级比较，LiqD的优先级最高，其它任务类型次之
            if (TaskType == TaskType.Chealing)
                return -1;
            if (other.TaskType == TaskType.Chealing)
                return 1;
            return TaskType.CompareTo(other.TaskType);
        }
    }

    class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _items = new List<T>();

        public int Count => _items.Count;

        public void Enqueue(T item)
        {
            _items.Add(item);
            //int i = _items.Count - 1;

            //while (i > 0)
            //{
            //    int parent = (i - 1) / 2;

            //    if (_items[i].CompareTo(_items[parent]) >= 0)
            //        break;

            //    T temp = _items[i];
            //    _items[i] = _items[parent];
            //    _items[parent] = temp;

            //    i = parent;
            //}
        }

        public T Dequeue()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            T item = _items[0];
            int lastIndex = _items.Count - 1;
            _items[0] = _items[lastIndex];
            _items.RemoveAt(0);

            int currentIndex = 0;

            //while (true)
            //{
            //    int leftChild = 2 * currentIndex + 1;
            //    int rightChild = 2 * currentIndex + 2;

            //    if (leftChild >= _items.Count)
            //        break;

            //    int smallerChild = leftChild;

            //    if (rightChild < _items.Count && _items[rightChild].CompareTo(_items[leftChild]) < 0)
            //        smallerChild = rightChild;

            //    if (_items[currentIndex].CompareTo(_items[smallerChild]) <= 0)
            //        break;

            //    T temp = _items[currentIndex];
            //    _items[currentIndex] = _items[smallerChild];
            //    _items[smallerChild] = temp;

            //    currentIndex = smallerChild;
            //}

            return item;
        }
    }
}
