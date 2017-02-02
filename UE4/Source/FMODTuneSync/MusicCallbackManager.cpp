// Fill out your copyright notice in the Description page of Project Settings.

#include "FMODTuneSync.h"
#include "MusicCallbackManager.h"

#include <map>
#include <string>
#include <functional>
#include <list>
#include "fmod_studio.hpp"
#include "fmod.hpp"
#include "FMODStudioModule.h"

FMOD_RESULT F_CALLBACK MusicCallbackManager::HandleCallbacks(FMOD_STUDIO_EVENT_CALLBACK_TYPE type, FMOD_STUDIO_EVENTINSTANCE *event, void *parameters)
{
    FMOD::Studio::EventInstance *instance = (FMOD::Studio::EventInstance *)event;
    
    if (type == FMOD_STUDIO_EVENT_CALLBACK_STOPPED)
    {
        // TODO: Handle event instance stop here
    }
    else if (type == FMOD_STUDIO_EVENT_CALLBACK_CREATE_PROGRAMMER_SOUND)
    {
        FMOD_STUDIO_PROGRAMMER_SOUND_PROPERTIES* props = (FMOD_STUDIO_PROGRAMMER_SOUND_PROPERTIES *)parameters;
        // TODO: Handle programmer sound creation here
    }
    else if (type == FMOD_STUDIO_EVENT_CALLBACK_DESTROY_PROGRAMMER_SOUND)
    {
        FMOD_STUDIO_PROGRAMMER_SOUND_PROPERTIES* props = (FMOD_STUDIO_PROGRAMMER_SOUND_PROPERTIES *)parameters;
        // TODO: Handle programmer sound destruction here

    } else if (type == FMOD_STUDIO_EVENT_CALLBACK_TIMELINE_MARKER)
    {
        FMOD_STUDIO_TIMELINE_MARKER_PROPERTIES* props = (FMOD_STUDIO_TIMELINE_MARKER_PROPERTIES*)parameters;
        
        std::list<std::function<void()>> toCall = MusicCallbackManager::getInstance().callbacks[props->name];
        
        for(std::function<void()> func : toCall)
        {
            func();
        }
    } else if (type == FMOD_STUDIO_EVENT_CALLBACK_TIMELINE_BEAT)
    {
        FMOD_STUDIO_TIMELINE_BEAT_PROPERTIES* props = (FMOD_STUDIO_TIMELINE_BEAT_PROPERTIES*)parameters;
        // TODO: Handle timeline beat events here
    }
    
    return FMOD_OK;
}

void MusicCallbackManager::RegisterCallback(std::string key, std::function<void()> func) {

    callbacks[key].push_back(func);

}

void MusicCallbackManager::initialize() {
    FMOD::Studio::System* StudioSystem = IFMODStudioModule::Get().GetStudioSystem(EFMODSystemContext::Runtime);
    if (StudioSystem)
    {
        FMOD::Studio::EventDescription* eventDescription = NULL;
        StudioSystem->getEvent("event:/MasterSlave", &eventDescription);
        
        FMOD::Studio::EventInstance* eventInstance = NULL;
        eventDescription->createInstance(&eventInstance);
        
        eventInstance->setCallback(MusicCallbackManager::HandleCallbacks,
                                   FMOD_STUDIO_EVENT_CALLBACK_TIMELINE_MARKER | FMOD_STUDIO_EVENT_CALLBACK_TIMELINE_BEAT |
                                   FMOD_STUDIO_EVENT_CALLBACK_SOUND_PLAYED | FMOD_STUDIO_EVENT_CALLBACK_SOUND_STOPPED);
        eventInstance->start();
    }
}

