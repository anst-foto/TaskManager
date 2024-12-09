using System;
using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    /*private string? _searchText;
    public string? SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }*/
    
    public ObservableCollection<Task> Tasks { get; } = [];
    public ObservableCollection<TaskPriority> Priorities { get; } = [];
    
    [Reactive] public string? SearchText { get; set; }
    [Reactive] public Task? SelectedTask { get; set; }
    [Reactive] public TaskPriority? SelectedPriority { get; set; }

    [Reactive] public string? Name { get; set; }
    [Reactive] public string? Description { get; set; }
    [Reactive] public DateTimeOffset? DateEnd { get; set; }
    
    public ReactiveCommand<Unit, Unit> CommandSearch { get; }
    public ReactiveCommand<Unit, Unit> CommandClearSearch { get; }
    
    public ReactiveCommand<Unit, Unit> CommandSave { get; }
    public ReactiveCommand<Unit, Unit> CommandDelete { get; }
    public ReactiveCommand<Unit, Unit> CommandClear { get; }

    public MainWindowViewModel()
    {
        Priorities.Add(TaskPriority.Low);
        Priorities.Add(TaskPriority.Medium);
        Priorities.Add(TaskPriority.High);
        
        var canExecuteSearch = this.WhenAnyValue(
            property1: p1 => p1.SearchText,
            x => !string.IsNullOrWhiteSpace(x));
        var canExecuteSave = this.WhenAnyValue(
            property1: p1 => p1.Name,
            property2: p2 => p2.Description,
            property3: p3 => p3.SelectedPriority,
            property4: p4 => p4.DateEnd,
            (p1, p2, p3, p4) => !string.IsNullOrWhiteSpace(p1) && 
                                !string.IsNullOrWhiteSpace(p2) && 
                                p3 != null && p4 != null);
        var canExecuteClear = this.WhenAnyValue(
            property1: p1 => p1.Name,
            property2: p2 => p2.Description,
            property3: p3 => p3.SelectedPriority,
            property4: p4 => p4.DateEnd,
            (p1, p2, p3, p4) => !string.IsNullOrWhiteSpace(p1) || 
                               !string.IsNullOrWhiteSpace(p2) || 
                               p3 != null || p4 != null);

        this.WhenValueChanged(x => x.SelectedTask)
            .Subscribe(x =>
            {
                Name = x?.Name;
                Description = x?.Description;
                DateEnd = x?.DateEnd;
                SelectedPriority = x?.Priority;
            });

        CommandSearch = ReactiveCommand.Create(
            execute: () =>
            {
                
            },
            canExecute: canExecuteSearch);
        
        CommandClearSearch = ReactiveCommand.Create(
            execute: () =>
            {
                SearchText = null;
            },
            canExecute: canExecuteSearch);

        CommandSave = ReactiveCommand.Create(
            execute: () =>
            {
                if (SelectedTask != null)
                {
                    //Update
                }
                else
                {
                    var task = new Task()
                    {
                        Name = Name!,
                        Description = Description!,
                        DateEnd = DateEnd!.Value.DateTime,
                        Priority = SelectedPriority!.Value
                    };
                    Tasks.Add(task);
                    
                    Clear();
                }
            },
            canExecute: canExecuteSave);

        CommandClear = ReactiveCommand.Create(
            execute: Clear,
            canExecute: canExecuteClear);
    }

    private void Clear()
    {
        SelectedTask = null;
        SelectedPriority = null;
        Name = null;
        Description = null;
        DateEnd = null;
    }
}