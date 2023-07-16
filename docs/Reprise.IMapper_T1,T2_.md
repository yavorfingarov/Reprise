### [Reprise](Reprise.md 'Reprise')

## IMapper<T1,T2> Interface

Specifies the contract to map objects.

```csharp
public interface IMapper<T1,T2>
```
#### Type parameters

<a name='Reprise.IMapper_T1,T2_.T1'></a>

`T1`

<a name='Reprise.IMapper_T1,T2_.T2'></a>

`T2`
### Methods

<a name='Reprise.IMapper_T1,T2_.Map(T1)'></a>

## IMapper<T1,T2>.Map(T1) Method

Maps a source object to a new destination object.

```csharp
T2 Map(T1 source);
```
#### Parameters

<a name='Reprise.IMapper_T1,T2_.Map(T1).source'></a>

`source` [T1](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T1 'Reprise.IMapper<T1,T2>.T1')

#### Returns
[T2](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T2 'Reprise.IMapper<T1,T2>.T2')

<a name='Reprise.IMapper_T1,T2_.Map(T1,T2)'></a>

## IMapper<T1,T2>.Map(T1, T2) Method

Maps a source object to an existing destination object.

```csharp
void Map(T1 source, T2 destination);
```
#### Parameters

<a name='Reprise.IMapper_T1,T2_.Map(T1,T2).source'></a>

`source` [T1](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T1 'Reprise.IMapper<T1,T2>.T1')

<a name='Reprise.IMapper_T1,T2_.Map(T1,T2).destination'></a>

`destination` [T2](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T2 'Reprise.IMapper<T1,T2>.T2')

<a name='Reprise.IMapper_T1,T2_.Map(T2)'></a>

## IMapper<T1,T2>.Map(T2) Method

Maps a source object to a new destination object.

```csharp
T1 Map(T2 source);
```
#### Parameters

<a name='Reprise.IMapper_T1,T2_.Map(T2).source'></a>

`source` [T2](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T2 'Reprise.IMapper<T1,T2>.T2')

#### Returns
[T1](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T1 'Reprise.IMapper<T1,T2>.T1')

<a name='Reprise.IMapper_T1,T2_.Map(T2,T1)'></a>

## IMapper<T1,T2>.Map(T2, T1) Method

Maps a source object to an existing destination object.

```csharp
void Map(T2 source, T1 destination);
```
#### Parameters

<a name='Reprise.IMapper_T1,T2_.Map(T2,T1).source'></a>

`source` [T2](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T2 'Reprise.IMapper<T1,T2>.T2')

<a name='Reprise.IMapper_T1,T2_.Map(T2,T1).destination'></a>

`destination` [T1](Reprise.IMapper_T1,T2_.md#Reprise.IMapper_T1,T2_.T1 'Reprise.IMapper<T1,T2>.T1')