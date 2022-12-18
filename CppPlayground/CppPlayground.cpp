#define UNICODE
#define _UNICODE
#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <shlobj.h>
#include <exdisp.h>
#include <shlwapi.h>
#include <atlbase.h>
#include <atlalloc.h>
#include <stdio.h>
#include <iostream>
using namespace std;

#define EXTERN_DLL_EXPORT extern "C" __declspec(dllexport)
class CCoInitialize {
public:
    CCoInitialize() : m_hr(CoInitialize(NULL)) { }
    ~CCoInitialize() { if (SUCCEEDED(m_hr)) CoUninitialize(); }
    operator HRESULT() const { return m_hr; }
    HRESULT m_hr;
};

void FindDesktopFolderView(REFIID riid, void** ppv)
{
    CComPtr<IShellWindows> spShellWindows;
    spShellWindows.CoCreateInstance(CLSID_ShellWindows);

    CComVariant vtLoc(CSIDL_DESKTOP);
    CComVariant vtEmpty;
    long lhwnd;
    CComPtr<IDispatch> spdisp;
    spShellWindows->FindWindowSW(
        &vtLoc, &vtEmpty,
        SWC_DESKTOP, &lhwnd, SWFO_NEEDDISPATCH, &spdisp);

    CComPtr<IShellBrowser> spBrowser;
    CComQIPtr<IServiceProvider>(spdisp)->
        QueryService(SID_STopLevelBrowser,
            IID_PPV_ARGS(&spBrowser));

    CComPtr<IShellView> spView;
    spBrowser->QueryActiveShellView(&spView);

    spView->QueryInterface(riid, ppv);
}

// CCoInitialize incorporated by reference
int __cdecl wmain(int argc, wchar_t** argv)
{
    CCoInitialize init;
    CComPtr<IFolderView2> spView;
    FindDesktopFolderView(IID_PPV_ARGS(&spView));
    CComPtr<IShellFolder2> spFolder;
    spView->GetFolder(IID_PPV_ARGS(&spFolder));
    spView->SetCurrentFolderFlags(FWF_AUTOARRANGE | FWF_SNAPTOGRID, 0);

    int id = -1;
    spView->GetFocusedItem(&id);
    CComHeapPtr<ITEMID_CHILD> item;
    spView->Item(id, &item);
    STRRET str2;
    spFolder->GetDisplayNameOf(item, SHGDN_NORMAL, &str2);
    CComHeapPtr<wchar_t> spszName2;
    StrRetToStr(&str2, item, &spszName2);
    wprintf(L"%ls\n",spszName2);


    CComPtr<IEnumIDList> spEnum;
    spView->Items(SVGIO_ALLVIEW, IID_PPV_ARGS(&spEnum));
    for (CComHeapPtr<ITEMID_CHILD> spidl;
        spEnum->Next(1, &spidl, nullptr) == S_OK;
        spidl.Free()) {
        STRRET str;
        spFolder->GetDisplayNameOf(spidl, SHGDN_NORMAL, &str);
        CComHeapPtr<wchar_t> spszName;
        StrRetToStr(&str, spidl, &spszName);

        POINT pt;
        spView->GetItemPosition(spidl, &pt);

        pt.x += (rand() % 50) - 2;
        pt.y += (rand() % 50) - 2;

        PCITEMID_CHILD apidl[1] = { spidl };

        wprintf(L"At %4d,%4d is %ls\n", pt.x, pt.y, spszName);
    }
    return 0;
}