// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

/**
 * 
 */

#include <map>
#include <string>
#include <functional>
#include <list>
#include "fmod_studio.h"
#include "fmod.h"

class FMODTUNESYNC_API MusicCallbackManager
{
    public:
        static MusicCallbackManager& getInstance()
        {
            static MusicCallbackManager instance;
            return instance;
        }
        
        void RegisterCallback(std::string key, std::function<void()> func);
        
        std::map<std::string, std::list<std::function<void()>>> callbacks;
    private:
        
        
        MusicCallbackManager() {
            initialize();
        };
        
        void initialize();

        static FMOD_RESULT F_CALLBACK HandleCallbacks(FMOD_STUDIO_EVENT_CALLBACK_TYPE type, FMOD_STUDIO_EVENTINSTANCE *event, void *parameters);
};
