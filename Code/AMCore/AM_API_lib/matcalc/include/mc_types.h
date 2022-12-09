// clang-format off
// basic types used in MatCalc
#ifndef MC_HANDLE
    #define MC_HANDLE   void *
#endif
#ifndef MC_INDEX
	// #include<QtGlobal>
    #if defined(__LP64__) || defined(_M_X64)
        // typedef quint64 native_unsigned_int_pointer_t;
        #define MC_INDEX    long
    #else
        typedef quint32 native_unsigned_int_pointer_t;
        #define MC_INDEX    int
    #endif
#endif

//platform specific ...
#if !defined(Q_OS_WIN32) && !defined(Q_OS_WIN64)

#ifndef ASSERT
//	#define ASSERT assert
// do NOT assert on UNIX systems!!!
	#define ASSERT 
#endif
#ifndef ASSERT_VALID
//	#define ASSERT_VALID assert
// do NOT assert on UNIX systems!!!
	#define ASSERT_VALID 
#endif
#endif


//#define HANDLE int THIS CAUSES TROUBLES WITH THE TYPEDEF Qt::HANDLE in Qt.
typedef int MC_HANDLE_2;

#ifndef Q_OS_MAC
#define LPVOID void *
#endif

#ifdef DLL_EXPORT
	#ifndef __GNUC__
		#define DECL_DLL_EXPORT extern "C" __declspec( dllexport )
	#else
		#define DECL_DLL_EXPORT extern "C" __attribute__ ((visibility ("default")))
	#endif
#else
	#define DECL_DLL_EXPORT extern "C"
#endif
