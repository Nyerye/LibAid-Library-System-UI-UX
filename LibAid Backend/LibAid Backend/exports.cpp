#include "mainHeader.h"


static HashTable ht;
static SnapshotStack* undoStack = nullptr;

extern "C" {

    __declspec(dllexport) void InitSystem() {
        initHashTable(&ht);
        undoStack = initSnapshotStack();
        loadDatabase(&ht, "database.txt");
    }

    __declspec(dllexport) void AddUser(const char* firstName, const char* lastName) {
        pushSnapshot(&ht, undoStack);

        //Construct a user on the fly and insert into hash table manually
        User* newUser = (User*)malloc(sizeof(User));
        strcpy_s(newUser->firstName, MAX_NAME_LEN, firstName);
        strcpy_s(newUser->lastName, MAX_NAME_LEN, lastName);
        newUser->userId = generateUserHash(lastName);
        newUser->next = nullptr;

        int index = newUser->userId % TABLE_SIZE;
        newUser->next = ht.users[index];
        ht.users[index] = newUser;

        char logMsg[MAX_LOG_LEN];
        snprintf(logMsg, sizeof(logMsg), "User %s %s added from GUI", firstName, lastName);
        logAction("AddUser", logMsg);

        syncDatabaseToFile(&ht, "database.txt");
    }

    __declspec(dllexport) void CleanupSystem() {
        freeSnapshotStack(&undoStack);
        freeHashTable(&ht);
    }
}
