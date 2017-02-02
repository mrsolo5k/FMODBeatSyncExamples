// Fill out your copyright notice in the Description page of Project Settings.

#include "FMODTuneSync.h"
#include "MusicReactable.h"
#include "MusicCallbackManager.h"


// Sets default values for this component's properties
UMusicReactable::UMusicReactable()
{
    MusicCallbackManager instance = MusicCallbackManager::getInstance();
    
    initialPosition = GetOwner()->GetActorLocation();
    
    UMusicReactable* self = this;
    
    instance.RegisterCallback("Marker1", [self] () {
        self->GetOwner()->SetActorLocation(self->initialPosition);
    });
    
    
  // Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
  // off to improve performance if you don't need them.
  PrimaryComponentTick.bCanEverTick = true;

  // ...
}


// Called when the game starts
void UMusicReactable::BeginPlay()
{
  Super::BeginPlay();

  // ...
  
}


// Called every frame
void UMusicReactable::TickComponent( float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction )
{
  Super::TickComponent( DeltaTime, TickType, ThisTickFunction );

  // ...
}

