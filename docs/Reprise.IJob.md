### [Reprise](Reprise.md 'Reprise')

## IJob Interface

Specifies the contract for running jobs.

```csharp
public interface IJob
```
### Methods

<a name='Reprise.IJob.Run(System.Threading.CancellationToken)'></a>

## IJob.Run(CancellationToken) Method

Runs the job.

```csharp
System.Threading.Tasks.Task Run(System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Reprise.IJob.Run(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')