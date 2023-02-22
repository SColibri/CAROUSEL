#pragma once

namespace AMFramework
{
	namespace Callback
	{
		// ======================================================
		//						GENERAL
		// ======================================================

		/// <summary>
		/// Message callback
		/// </summary>
		typedef void(__stdcall* MessageCallbackF)(char* message);
		
		/// <summary>
		/// Report the progress of any action currently running
		/// </summary>
		typedef void(__stdcall* ProgressUpdateCallbackF)(char* message, double progress);
		
		/// <summary>
		/// Reports any errors
		/// </summary>
		typedef void(__stdcall* ErrorCallbackF)(char* message);

		// ======================================================
		//						SCRIPTS
		// ======================================================
		
		/// <summary>
		/// Notifies when a script ended.
		/// </summary>
		typedef void(__stdcall* ScriptFinishedCallbackF)(char* message);
	}
}